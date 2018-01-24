import {Component, OnInit} from '@angular/core';
import {VotingService} from '../../services/voting/voting-service';
import {ProjectCardType} from '../../services/project-card-type';
import {BalanceService} from '../../services/balance/balance.service';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {VotingSprint} from '../../services/voting/voting-sprint';
import * as timespan from 'timespan';

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

  constructor(private balanceService: BalanceService,
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadSprintAsync();

    this.updateRemainingTime();

    const options = {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    };

    this.dateFrom = this.currentSprint.startDate.toLocaleDateString('en-us', options);
    this.dateTo = this.currentSprint.endDate.toLocaleDateString('en-us', options);

    setInterval(() => this.updateRemainingTime(), 1000);
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
