import {Component, OnInit} from '@angular/core';
import {VotingService} from '../../services/voting/voting-service';
import {ProjectCardType} from '../../services/project-card-type';
import {BalanceService} from '../../services/balance/balance.service';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {VotingSprint} from '../../services/voting/voting-sprint';
import * as timespan from 'timespan';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.css']
})
export class VotingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public projects: Array<ProjectCardData> = [];
  public currentSprint: VotingSprint;

  public remainingDays: number;
  public remainingHours: number;
  public remainingMinutes: number;
  public remainingSeconds: number;

  public dateFrom: string;
  public dateTo: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadSprintAsync();

    this.updateRemainingTime();

    this.dateFrom = moment(this.currentSprint.startDate).format('MMMM D, Y');
    this.dateTo = moment(this.currentSprint.endDate).format('MMMM D, Y');

    setInterval(() => this.updateRemainingTime(), 1000);
  }

  async navigateToCompleted() {
    await this.router.navigate([Paths.CompletedVotings]);
  }

  private updateRemainingTime(): void {
    const remaining = timespan.fromDates(new Date(), this.currentSprint.endDate);
    this.remainingDays = remaining.days;
    this.remainingHours = remaining.hours;
    this.remainingMinutes = remaining.minutes;
    this.remainingSeconds = remaining.seconds;
  }

  private async loadSprintAsync() {
    this.currentSprint = await this.sprintService.getCurrentSprintAsync();
    this.projects = this.currentSprint.projects.map(p => ProjectCardData.fromProject(p));
  }
}
