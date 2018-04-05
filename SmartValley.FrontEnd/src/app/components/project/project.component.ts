import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {UserContext} from '../../services/authentication/user-context';
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
  public selectedTab = 0;

  public isAuthor = false;
  public isScoringApplicationTabAvailable = true;

  constructor(private projectApiClient: ProjectApiClient,
              private router: Router,
              private route: ActivatedRoute,
              private userContext: UserContext) {
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

      const currentUser = await this.userContext.getCurrentUser();
      if (!isNullOrUndefined(currentUser) && this.project.authorId === currentUser.id) {
        this.isAuthor = true;
      }
    }
  }

  public async navigateToApplicationScoringAsync(): Promise<void> {
    await this.router.navigate(['/' + Paths.ScoringApplication + '/' + this.projectId]);
  }

  public async navigateToPaymentAsync(): Promise<void> {
    await this.router.navigate([Paths.Project + '/' + this.projectId + '/payment']);
  }
}
