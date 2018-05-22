import {Component} from '@angular/core';
import {ProjectResponse} from '../../../api/project/project-response';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {LazyLoadEvent} from 'primeng/api';
import {ProjectQuery} from '../../../api/project/project-query';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {MatCheckboxChange} from '@angular/material';

@Component({
  selector: 'app-admin-projects-list',
  templateUrl: './admin-projects-list.component.html',
  styleUrls: ['./admin-projects-list.component.css']
})
export class AdminProjectsListComponent {

  public totalRecords: number;
  public loading = false;
  public offset = 0;
  public pageSize = 10;
  public projects: ProjectResponse[] = [];

  public ScoringStatus = ScoringStatus;
  public currentScoringStatuses?: ScoringStatus[] = [];

  constructor(private projectApiClient: ProjectApiClient) {
  }

  async changeStatusAsync(status: number, event: MatCheckboxChange) {
    if (event.checked) {
      this.currentScoringStatuses.push(status);
    } else {
      const index = this.currentScoringStatuses.indexOf(status, 0);
      if (index > -1) {
        this.currentScoringStatuses.splice(index, 1);
      }
    }
    await this.renderingProjectsAsync();
  }

  private async renderingProjectsAsync() {
    this.loading = true;
    const response = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: this.offset,
      count: this.pageSize,
      onlyScored: false,
      isPrivate: true,
      scoringStatuses: this.currentScoringStatuses
    });
    this.totalRecords = response.totalCount;
    this.projects = response.items;
    this.loading = false;
  }

  public async updateProjects(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.renderingProjectsAsync();
  }
}
