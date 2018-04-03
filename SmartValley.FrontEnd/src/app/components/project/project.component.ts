import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {DialogService} from '../../services/dialog-service';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {isNullOrUndefined} from 'util';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  public tabItems: string[] = ['about', 'application'];
  public isProjectExists = false;
  public projectId: number;
  public project: ProjectSummaryResponse;
  public editProjectsLink = Paths.ProjectEdit;
  public isApplicationScoringExist = false;
  public selectedTab = 0;

  constructor(private projectApiClient: ProjectApiClient,
              private dialogService: DialogService,
              private router: Router,
              private route: ActivatedRoute,
              private scoringApplicationApiClient: ScoringApplicationApiClient) {
  }

  public async ngOnInit() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    const selectedTabName = this.route.snapshot.paramMap.get('tab');
    if (!isNullOrUndefined(selectedTabName) && this.tabItems.includes(selectedTabName)) {
      this.selectedTab = this.tabItems.indexOf(selectedTabName);
    }

    this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    if (this.project) {
      this.isProjectExists = true;
    }
    const applicationScoringRequest = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.projectId);
    this.isApplicationScoringExist = !!applicationScoringRequest.created;
  }

  public navigateToApplicationScoringAsync(): Promise<boolean> {
    return this.router.navigate(['/' + Paths.ScoringApplication + '/' + this.projectId]);
  }

  public showWaitingModal() {
    this.dialogService.showWaitingModal();
  }
}
