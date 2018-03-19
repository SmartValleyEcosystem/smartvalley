import {Component, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {ProjectService} from '../../../services/project/project-service';
import {BlockiesService} from '../../../services/blockies-service';
import {Constants} from '../../../constants';
import {ProjectCardType} from '../../../services/project-card-type';
import {ProjectCardData} from './project-card-data';
import {VotingStatus} from '../../../services/voting-status.enum';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {isNullOrUndefined} from 'util';
import * as timespan from 'timespan';
import {VotingService} from '../../../services/voting/voting-service';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent implements OnInit {
  public ProjectCardType = ProjectCardType;
  public ScoringStatus = ScoringStatus;
  public VotingStatus = VotingStatus;
  public projectImageUrl: string;
  public projectTokensPercent: number;

  public votingRemainingDays: number;
  public votingRemainingHours: number;
  public votingRemainingMinutes: number;
  public votingRemainingSeconds: number;

  @Input() public data: ProjectCardData;
  @Input() public type: ProjectCardType;
  @Input() public totalVotes: number;
  @Input() public votingAcceptanceThreshold: number;

  constructor(private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private votingService: VotingService) {
  }

  public ngOnInit(): void {
    const address = this.data.address;
    if (!isNullOrUndefined(address)) {
      this.projectImageUrl = this.blockiesService.getImageForAddress(address);

      if (this.data.votingStatus === VotingStatus.InProgress) {
        this.updateVotingRemainingTime();
        setInterval(() => this.updateVotingRemainingTime(), 1000);
      }

      this.projectTokensPercent = Math.round(100 * this.data.projectVote / this.totalVotes);
    }
  }

  public showProject(): void {
    this.router.navigate([Paths.Scoring + '/' + this.data.id], {queryParams: {areaType: this.data.areaType}});
  }

  public showVotingDetails(): void {
    this.router.navigate([Paths.Voting + '/' + this.data.id]);
  }

  public async voteForProjectAsync(): Promise<void> {
    await this.votingService.submitVoteToCurrentSprintAsync(this.data.externalId, this.data.name);
  }

  public showReport(): void {
    this.router.navigate([Paths.Report + '/' + this.data.id], {queryParams: {tab: Constants.ReportFormTab}});
  }

  private updateVotingRemainingTime(): void {
    if (isNullOrUndefined(this.data.votingEndDate)) {
      return;
    }

    const remaining = timespan.fromDates(new Date(), this.data.votingEndDate);
    this.votingRemainingDays = remaining.days;
    this.votingRemainingHours = remaining.hours;
    this.votingRemainingMinutes = remaining.minutes;
    this.votingRemainingSeconds = remaining.seconds;
  }
}
