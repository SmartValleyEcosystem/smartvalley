import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../services/project/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {Paths} from '../../paths';
import {VotingSprint} from '../../services/voting/voting-sprint';
import {VotingService} from '../../services/voting/voting-service';
import {VotingContractClient} from '../../services/contract-clients/voting-contract-client';
import {DialogService} from '../../services/dialog-service';
import * as timespan from 'timespan';

@Component({
  selector: 'app-voting-card',
  templateUrl: './voting-card.component.html',
  styleUrls: ['./voting-card.component.css']
})
export class VotingCardComponent implements OnInit {
  public projectDetails: ProjectDetailsResponse;
  public projectImageUrl: string;

  private projectId: number;
  public currentSprint: VotingSprint;

  public remainingDays: number;
  public remainingHours: number;
  public remainingMinutes: number;
  public remainingSeconds: number;

  public isVotedByMe: boolean;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute,
              private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private votingService: VotingService) {
  }

  public async ngOnInit() {
    await this.loadInitialData();
    await this.loadSprintAsync();
    this.updateRemainingTime();
    setInterval(() => this.updateRemainingTime(), 1000);
  }

  private updateRemainingTime(): void {
    const remaining = timespan.fromDates(new Date(), this.currentSprint.endDate);
    this.remainingDays = remaining.days;
    this.remainingHours = remaining.hours;
    this.remainingMinutes = remaining.minutes;
    this.remainingSeconds = remaining.seconds;
  }

  private async loadInitialData(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.projectDetails = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.getImageUrl();
  }

  private getImageUrl(): string {
    const address = this.projectDetails.scoringContractAddress ? this.projectDetails.scoringContractAddress : this.projectDetails.authorAddress;
    return this.blockiesService.getImageForAddress(address);
  }

  private async loadSprintAsync() {
    this.currentSprint = await this.votingService.getCurrentSprintAsync();
    this.isVotedByMe = this.currentSprint.projects.find(p => p.externalId === this.projectDetails.externalId).isVotedByMe;
  }

  public async voteAsync(): Promise<void> {
    await this.votingService.submitVoteAsync(
      this.currentSprint.address,
      this.projectDetails.externalId,
      this.projectDetails.name,
      this.currentSprint.voteBalance,
      this.currentSprint.endDate);

    await this.loadSprintAsync();
  }

  public async navigateToVoting() {
    await this.router.navigate([Paths.Voting]);
  }
}
