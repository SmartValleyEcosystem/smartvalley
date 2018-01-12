import {Injectable} from '@angular/core';
import {Project} from '../project';
import {Sprint} from './sprint';
import {ProjectApiClient} from '../../api/project/project-api-client';

@Injectable()
export class SprintService {

  constructor(private projectApiClient: ProjectApiClient) {

  }

  public async hasActiveSprintAsync(): Promise<boolean> {
    const currentSprint = await this.getCurrentSprintAsync();
    return !!currentSprint;
  }


  public async getCurrentSprintAsync(): Promise<Sprint> {
    const response = await this.projectApiClient.getScoredProjectsAsync();
    const projects = [];
    for (const projectResponse of response.items) {
      projects.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        address: projectResponse.address
      });
    }

    return <Sprint>{
      number: 1,
      projects: projects,
      startDate: this.getWeekStartDate(new Date()),
      endDate: this.getWeekEndDate(new Date())
    };
  }

  private getWeekDay(date: Date): number {
    const day = date.getDay();
    if (day !== 0) {
      return day;
    } else {
      return 7;
    }
  }

  private getWeekStartDate(date: Date): Date {
    date.setDate(date.getDay() - this.getWeekDay(date) + 8);
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return date;
  }

  private getWeekEndDate(date: Date): Date {
    date.setDate(date.getDay() - this.getWeekDay(date) + 14);
    date.setHours(23);
    date.setMinutes(59);
    date.setSeconds(59);
    date.setMilliseconds(0);
    return date;
  }
}
