import {Component, OnInit} from '@angular/core';
import {VotingService} from '../../services/voting/voting-service';
import {ProjectCardType} from '../../services/project-card-type';
import {Balance} from '../../services/balance/balance';
import {BalanceService} from '../../services/balance/balance.service';
import {VotingSprint} from '../../services/voting/voting-sprint';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.css']
})
export class VotingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;

  public currentSprint: VotingSprint;

  public endDays: number;
  public endHours: number;
  public endMinutes: number;
  public endSeconds: number;

  public dateFrom: string;
  public dateTo: string;

  constructor(private balanceService: BalanceService,
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadSprintAsync();
    this.updateDate();
    setInterval(() => this.updateDate(), 1000);
  }

  private updateDate(): void {
    const currentDate = new Date();
    this.endDays = this.currentSprint.endDate.getDate() - currentDate.getDate();
    if (this.endDays < 0) {
      this.endDays = new Date(currentDate.getFullYear(), currentDate.getMonth(), 0).getDate() + this.endDays;
    }
    this.endHours = this.currentSprint.endDate.getHours() - currentDate.getHours();
    this.endMinutes = this.currentSprint.endDate.getMinutes() - currentDate.getMinutes();
    this.endSeconds = this.currentSprint.endDate.getSeconds() - currentDate.getSeconds();

    const options = {
      year: 'numeric', month: 'long',
      day: 'numeric'
    };

    this.dateFrom = this.currentSprint.startDate.toLocaleDateString('en-us', options);
    this.dateTo = this.currentSprint.endDate.toLocaleDateString('en-us', options);
  }

  private async loadSprintAsync() {
    this.currentSprint = await this.sprintService.getCurrentSprintAsync();
  }

}
