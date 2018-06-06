import {Component} from '@angular/core';
import {ProjectResponse} from '../../../api/project/project-response';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {LazyLoadEvent, SelectItem} from 'primeng/api';
import {ProjectQuery} from '../../../api/project/project-query';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {TranslateService} from '@ngx-translate/core';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';

@Component({
  selector: 'app-admin-projects-list',
  templateUrl: './admin-projects-list.component.html',
  styleUrls: ['./admin-projects-list.component.css']
})
export class AdminProjectsListComponent {

  public totalRecords: number;
  public loading = true;
  public offset = 0;
  public pageSize = 10;
  public projects: ProjectResponse[] = [];

  public statuses: SelectItem[] = [];
  selectedStatuses: any[] = [];

  public ScoringStatus = ScoringStatus;

  constructor(private router: Router,
              private translateService: TranslateService,
              private projectApiClient: ProjectApiClient) {

    this.statuses = [
      {label: this.translateService.instant('AdminProjects.New'), value: ScoringStatus.FillingApplication},
      {label: this.translateService.instant('AdminProjects.InProgress'), value: ScoringStatus.InProgress},
      {label: this.translateService.instant('AdminProjects.Finished'), value: ScoringStatus.Finished}
    ];
  }

  async onCheckAsync() {
    await this.renderingProjectsAsync();
  }

  private async renderingProjectsAsync() {
    const response = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: this.offset,
      count: this.pageSize,
      onlyScored: false,
      isPrivate: true,
      scoringStatuses: this.selectedStatuses.map(i => <ScoringStatus>i)
    });
    this.totalRecords = response.totalCount;
    this.projects = response.items;
    this.loading = false;
  }

  public async updateProjects(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.renderingProjectsAsync();
  }

  public async navigateToEditScoring(projectId: number) {
    this.router.navigate([Paths.Project + `/${projectId}/edit-scoring`]);
  }

  public async navigateToProject(projectId: number) {
    await this.router.navigate([Paths.Project + '/' + projectId]);
  }
}
