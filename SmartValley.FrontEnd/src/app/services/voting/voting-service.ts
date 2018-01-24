import {Injectable} from '@angular/core';
import {Project} from '../project';
import {VotingSprint} from './voting-sprint';
import {VotingApiClient} from '../../api/voting/voting-api-client';
import * as moment from 'moment';
import {AuthenticationService} from '../authentication/authentication-service';
import {VotingContractClient} from '../contract-clients/voting-contract-client';
import {Paths} from '../../paths';
import {BalanceService} from '../balance/balance.service';
import {DialogService} from '../dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {Web3Service} from '../web3-service';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class VotingService {

  constructor(private votingApiClient: VotingApiClient,
              private authenticationService: AuthenticationService,
              private votingContractClient: VotingContractClient,
              private dialogService: DialogService,
              private translateService: TranslateService,
              private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private balanceService: BalanceService) {

  }

  public async hasActiveSprintAsync(): Promise<boolean> {
    const currentSprint = await this.getCurrentSprintAsync();
    return !!currentSprint;
  }

  public async getCurrentSprintAsync(): Promise<VotingSprint> {
    const lastSprintResponse = await this.votingApiClient.getLastVotingSprintAsync();
    if (!lastSprintResponse.doesExist) {
      return null;
    }

    const projects = [];
    for (const projectVoteResponse of lastSprintResponse.lastSprint.projects) {
      projects.push(<Project>{
        id: projectVoteResponse.id,
        externalId: projectVoteResponse.externalId,
        name: projectVoteResponse.name,
        area: projectVoteResponse.area,
        country: projectVoteResponse.country,
        description: projectVoteResponse.description,
        address: projectVoteResponse.author,
        isVotedByMe: this.authenticationService.isAuthenticated() ? projectVoteResponse.isVotedByMe : null
      });
    }

    return <VotingSprint> {
      address: lastSprintResponse.lastSprint.address,
      projects: projects,
      voteBalance: lastSprintResponse.lastSprint.voteBalance,
      startDate: moment(lastSprintResponse.lastSprint.startDate).toDate(),
      endDate: moment(lastSprintResponse.lastSprint.endDate).toDate()
    };
  }

  public async getCurrentSprintAndSubmitVoteAsync(projectExternalId: string, projectName: string): Promise<void> {
    const currentSprint = await this.getCurrentSprintAsync();

    return await this.submitVoteAsync(
      currentSprint.address,
      projectExternalId,
      projectName,
      currentSprint.voteBalance,
      currentSprint.endDate
    );
  }

  public async submitVoteAsync(votingSprintAddress: string,
                               projectExternalId: string,
                               projectName: string,
                               currentVoteBalance: number,
                               currentSprintEndDate: Date): Promise<void> {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {

      const amount = await this.dialogService.showVoteDialogAsync(
        projectName,
        this.balanceService.balance.availableBalance,
        currentVoteBalance,
        currentSprintEndDate);

      if (amount === undefined || amount === 0) {
        return;
      }

      const transactionHash = await this.votingContractClient.submitVoteAsync(
        votingSprintAddress,
        projectExternalId,
        amount);

      const transactionDialog = this.dialogService.showTransactionDialog(this.translateService.instant(
        'VotingService.VoteTransactionDialog'),
        transactionHash);

      try {
        await this.web3Service.waitForConfirmationAsync(transactionHash);
        this.notificationsService.success(this.translateService.instant('VotingService.Success'));
      } catch (e) {
        this.notificationsService.error(this.translateService.instant('VotingService.Error'));
      }

      transactionDialog.close();
    }
  }
}
