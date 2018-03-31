import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {DialogService} from '../../services/dialog-service';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  public isProjectExists = false;
  public projectId: number;
  public project: ProjectSummaryResponse;
  public editProjectsLink = Paths.ProjectEdit;
  public isApplicationScoringExist = false;

  constructor(private projectApiClient: ProjectApiClient,
              private dialogService: DialogService,
              private router: Router,
              private route: ActivatedRoute,
              private scoringApplicationApiClient: ScoringApplicationApiClient) { }

  public async ngOnInit() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    if ( this.project ) {
      this.isProjectExists = true;
    }
    let applicationScoringRequest = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.projectId);
    this.isApplicationScoringExist = applicationScoringRequest.created ? true : false;
  }

  public navigateToApplicationScoring() {
    this.router.navigate(['/' + Paths.ScoringApplication + '/' + this.projectId]);
  }

  public showWaitingModal() {
    this.dialogService.showWaitingModal();
  }

}
