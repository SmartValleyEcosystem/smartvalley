import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {ProjectApiClient} from '../../api/project/project-api-client';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.css']
})
export class VotingComponent implements OnInit {

  public projects: Array<Project>;

  public endDays: number;
  public endHours: number;
  public endMinutes: number;
  public endSeconds: number;
  public sprintNumber: number;

  public voteBalance = 0;

  public dateFrom: string;
  public dateTo: string;

  constructor(private projectApiClient: ProjectApiClient) {
    this.sprintNumber = 1;
    this.voteBalance = 250;
  }

  ngOnInit(): void {
    this.initializeProjectsCollection();
    this.updateDate();
    setInterval(() => this.updateDate(), 1000);
  }

  private updateDate(): void {
    const currentDate = new Date();
    const endDate = this.getWeekEndDate(new Date());
    this.endDays = 7 - currentDate.getDay();
    this.endHours = endDate.getHours() - currentDate.getHours();
    this.endMinutes = endDate.getMinutes() - currentDate.getMinutes();
    this.endSeconds = endDate.getSeconds() - currentDate.getSeconds();


    const options = {
      year: 'numeric', month: 'long',
      day: 'numeric'
    };

    this.dateFrom = this.getWeekStartDate(new Date()).toLocaleDateString('en-us', options);
    this.dateTo = this.getWeekStartNewDate(new Date()).toLocaleDateString('en-us', options);
  }

  private async initializeProjectsCollection() {
    const response = await this.projectApiClient.getScoredProjectsAsync();
    this.projects = [];
    for (const projectResponse of response.items) {
      this.projects.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        address: projectResponse.address
      });
    }
  }

  // Returns the ISO day of week
  private getWeekDay(date: Date): number {
    const day = date.getDay();
    if (day !== 0) {
      return day;
    } else {
      return 7;
    }
  }
// Returns current week end date
  private getWeekStartDate(date: Date): Date {
    date.setDate(date.getDay() - this.getWeekDay(date) + 8);
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return date;
  }

// Returns current week end date
  private getWeekStartNewDate(date: Date): Date {
    date.setDate(date.getDay() - this.getWeekDay(date) + 15);
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return date;
  }

// Returns current week end date
  private getWeekEndDate(date: Date): Date {
    date.setDate(date.getDay() - this.getWeekDay(date) + 14);
    date.setHours(23);
    date.setMinutes(59);
    date.setSeconds(59);
    date.setMilliseconds(0);
    return date;
  }

}
