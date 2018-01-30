import {EventEmitter, Injectable} from '@angular/core';
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
import {ProjectVoteResponse} from '../../api/voting/project-vote-response';
import {VotingSprintResponse} from '../../api/voting/voting-sprint-response';

@Injectable()
export class VotingService {

  public voteSubmitted: EventEmitter<any> = new EventEmitter<any>();

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
    const currentSprintResponse = await this.votingApiClient.getCurrentVotingSprintAsync();
    return currentSprintResponse.doesExist;
  }

  public async hasCompletedSprintsAsync(): Promise<boolean> {
    const currentSprintResponse = await this.votingApiClient.getСompletedSprintsAsync();
    return currentSprintResponse.items.length > 0;
  }

  public async getSprintByAddressAsync(address: string): Promise<VotingSprint> {
    const response = await this.votingApiClient.getVotingSprintByAddressAsync(address);
    if (!response.doesExist) {
      return null;
    }
    return this.createVotingSprint(response.sprint);
  }

  public async getCurrentSprintAsync(): Promise<VotingSprint> {
    const response = await this.votingApiClient.getCurrentVotingSprintAsync();
    if (!response.doesExist) {
      return null;
    }
    return this.createVotingSprint(response.sprint);
  }

  public async submitVoteToCurrentSprintAsync(projectExternalId: string, projectName: string): Promise<void> {
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
    if (!await this.authenticationService.authenticateAsync()) {
      return;
    }

    const amount = await this.getVotingTokenAmountAsync(
      projectName,
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
      this.voteSubmitted.emit();
    } catch (e) {
      this.notificationsService.error(this.translateService.instant('VotingService.Error'));
    }
    transactionDialog.close();
    await this.balanceService.updateBalanceAsync();
  }

  private async getVotingTokenAmountAsync(projectName: string,
                                          currentVoteBalance: number,
                                          currentSprintEndDate: Date): Promise<number> {
    if (!isNullOrUndefined(currentVoteBalance) && currentVoteBalance > 0) {
      return currentVoteBalance;
    }

    return await this.dialogService.showVoteDialogAsync(
      projectName,
      this.balanceService.balance.availableBalance,
      currentVoteBalance,
      currentSprintEndDate);
  }

  private createVotingSprint(votingSprintResponse: VotingSprintResponse): VotingSprint {
    const isAuthenticated = this.authenticationService.isAuthenticated();
    return <VotingSprint> {
      address: votingSprintResponse.address,
      projects: votingSprintResponse.projects.map(p => this.createProject(p, isAuthenticated)),
      voteBalance: votingSprintResponse.voteBalance,
      startDate: moment(votingSprintResponse.startDate).toDate(),
      endDate: moment(votingSprintResponse.endDate).toDate(),
      maximumScore: votingSprintResponse.maximumScore,
      acceptanceThreshold: votingSprintResponse.acceptanceThreshold,
      number: votingSprintResponse.number
    };
  }

  private createProject(projectVoteResponse: ProjectVoteResponse, isAuthenticated: boolean): Project {
    return <Project>{
      id: projectVoteResponse.id,
      externalId: projectVoteResponse.externalId,
      name: projectVoteResponse.name,
      area: projectVoteResponse.area,
      country: projectVoteResponse.country,
      description: projectVoteResponse.description,
      address: projectVoteResponse.author,
      isVotedByMe: isAuthenticated ? projectVoteResponse.isVotedByMe : null,
      myVoteTokensAmount: projectVoteResponse.myVoteTokenAmount,
      totalTokenVote: projectVoteResponse.totalTokenAmount,
      votingStatus: projectVoteResponse.votingStatus
    };
  }
}
