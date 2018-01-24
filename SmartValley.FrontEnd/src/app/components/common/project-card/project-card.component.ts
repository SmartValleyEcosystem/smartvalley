import {Component, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {ProjectService} from '../../../services/project-service';
import {BlockiesService} from '../../../services/blockies-service';
import {Constants} from '../../../constants';
import {ProjectCardType} from '../../../services/project-card-type';
import {ProjectCardData} from './project-card-data';
import {VotingStatus} from '../../../services/voting-status.enum';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {isNullOrUndefined} from 'util';
import * as timespan from 'timespan';

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

  public votingRemainingDays: number;
  public votingRemainingHours: number;
  public votingRemainingMinutes: number;
  public votingRemainingSeconds: number;

  @Input() public data: ProjectCardData;
  @Input() public type: ProjectCardType;

  constructor(private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService) {
  }

  public ngOnInit(): void {
    const address = this.data.address ? this.data.address : this.data.author;
    this.projectImageUrl = this.blockiesService.getImageForAddress(address);

    this.updateVotingRemainingTime();
    setInterval(() => this.updateVotingRemainingTime(), 1000);
  }

  public showProject(id: number): void {
    this.router.navigate([Paths.Scoring + '/' + id], {queryParams: {expertiseArea: this.data.expertiseArea}});
  }

  public voteForProject(id: number): void {
    this.router.navigate([Paths.Voting + '/' + id]);
  }

  public showReport(id: number): void {
    this.router.navigate([Paths.Report + '/' + id], {queryParams: {tab: Constants.ReportFormTab}});
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
