import {Injectable} from '@angular/core';
import {Project} from '../project';
import {VotingSprint} from './voting-sprint';
import {VotingApiClient} from '../../api/voting/voting-api-client';
import * as moment from 'moment';
import {AuthenticationService} from '../authentication/authentication-service';
import {VotingContractClient} from '../contract-clients/voting-contract-client';
import {BalanceService} from '../balance/balance.service';
import {DialogService} from '../dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {Web3Service} from '../web3-service';
import {NotificationsService} from 'angular2-notifications';
import {isNullOrUndefined} from 'util';

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

  public async getSprintByAddressAsync(address: string): Promise<VotingSprint> {
    const votingSprintResponse = (await this.votingApiClient.getVotingSprintByAddressAsync(address));

    if (!votingSprintResponse.doesExist) {
      return null;
    }

    const sprint = votingSprintResponse.sprint;

    const projects = [];
    for (const projectVoteResponse of sprint.projects) {
      projects.push(<Project>{
        id: projectVoteResponse.id,
        externalId: projectVoteResponse.externalId,
        name: projectVoteResponse.name,
        area: projectVoteResponse.area,
        country: projectVoteResponse.country,
        description: projectVoteResponse.description,
        address: projectVoteResponse.author,
        isVotedByMe: this.authenticationService.isAuthenticated() ? projectVoteResponse.isVotedByMe : null,
        myVoteTokensAmount: projectVoteResponse.myVoteTokenAmount,
        totalTokenVote: projectVoteResponse.totalTokenAmount
      });
    }

    return <VotingSprint> {
      address: sprint.address,
      projects: projects,
      voteBalance: sprint.voteBalance,
      startDate: moment(sprint.startDate).toDate(),
      endDate: moment(sprint.endDate).toDate(),
      maximumScore: sprint.maximumScore,
      acceptanceThreshold: sprint.acceptanceThreshold
    };
  }

  public async getCurrentSprintAsync(): Promise<VotingSprint> {
    const currentSprintResponse = await this.votingApiClient.getCurrentVotingSprintAsync();
    if (!currentSprintResponse.doesExist) {
      return null;
    }

    const projects = [];
    for (const projectVoteResponse of currentSprintResponse.sprint.projects) {
      projects.push(<Project>{
        id: projectVoteResponse.id,
        externalId: projectVoteResponse.externalId,
        name: projectVoteResponse.name,
        area: projectVoteResponse.area,
        country: projectVoteResponse.country,
        description: projectVoteResponse.description,
        address: projectVoteResponse.author,
        isVotedByMe: this.authenticationService.isAuthenticated() ? projectVoteResponse.isVotedByMe : null,
        votingStatus: projectVoteResponse.votingStatus
      });
    }

    return <VotingSprint> {
      address: currentSprintResponse.sprint.address,
      projects: projects,
      voteBalance: currentSprintResponse.sprint.voteBalance,
      startDate: moment(currentSprintResponse.sprint.startDate).toDate(),
      endDate: moment(currentSprintResponse.sprint.endDate).toDate(),
      number: currentSprintResponse.sprint.number
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
    if (await this.authenticationService.authenticateAsync()) {
      const amount = await this.dialogService.showVoteDialogAsync(
        projectName,
        this.balanceService.balance.availableBalance,
        currentVoteBalance,
        currentSprintEndDate);

      if (isNullOrUndefined(amount) || amount === 0) {
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
      await this.balanceService.updateBalanceAsync();
    }
  }
}
