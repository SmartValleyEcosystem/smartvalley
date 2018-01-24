import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {Paths} from '../../paths';
import {VotingSprint} from '../../services/voting/voting-sprint';
import {VotingService} from '../../services/voting/voting-service';
import * as timespan from 'timespan';

@Component({
  selector: 'app-voting-card',
  templateUrl: './voting-card.component.html',
  styleUrls: ['./voting-card.component.css']
})
export class VotingCardComponent implements OnInit {
  public details: ProjectDetailsResponse;
  public projectImageUrl: string;

  private projectId: number;
  public currentSprint: VotingSprint;

  public remainingDays: number;
  public remainingHours: number;
  public remainingMinutes: number;
  public remainingSeconds: number;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute,
              private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private sprintService: VotingService) {
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
    this.details = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.getImageUrl();
  }

  private getImageUrl(): string {
    const address = this.details.projectAddress ? this.details.projectAddress : this.details.authorAddress;
    return this.blockiesService.getImageForAddress(address);
  }

  private async loadSprintAsync() {
    this.currentSprint = await this.sprintService.getCurrentSprintAsync();
  }

  public async vote() {
  }

  public async navigateToVoting() {
    await this.router.navigate([Paths.Voting]);
  }
}
