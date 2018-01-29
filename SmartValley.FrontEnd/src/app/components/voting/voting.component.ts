import {Component, OnInit} from '@angular/core';
import {VotingService} from '../../services/voting/voting-service';
import {ProjectCardType} from '../../services/project-card-type';
import {BalanceService} from '../../services/balance/balance.service';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {VotingSprint} from '../../services/voting/voting-sprint';
import * as timespan from 'timespan';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication-service';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.css']
})
export class VotingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public projects: Array<ProjectCardData> = [];
  public sprint: VotingSprint;

  public remainingDays: number;
  public remainingHours: number;
  public remainingMinutes: number;
  public remainingSeconds: number;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private votingService: VotingService,
              private authenticationService: AuthenticationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadSprintDataAsync();
    setInterval(() => this.updateRemainingTime(), 1000);

    this.votingService.voteSubmitted.subscribe(() => this.loadSprintDataAsync());
    this.authenticationService.accountChanged.subscribe(() => this.loadSprintDataAsync());
  }

  async navigateToCompleted() {
    await this.router.navigate([Paths.CompletedVotings]);
  }

  private updateRemainingTime(): void {
    const remaining = timespan.fromDates(new Date(), this.sprint.endDate);
    this.remainingDays = remaining.days;
    this.remainingHours = remaining.hours;
    this.remainingMinutes = remaining.minutes;
    this.remainingSeconds = remaining.seconds;
  }

  private async loadSprintDataAsync(): Promise<void> {
    this.sprint = await this.votingService.getCurrentSprintAsync();
    this.projects = this.sprint.projects.map(p => ProjectCardData.fromProject(p));

    this.updateRemainingTime();
  }
}
