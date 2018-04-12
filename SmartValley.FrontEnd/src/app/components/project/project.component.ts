import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {UserContext} from '../../services/authentication/user-context';
import {isNullOrUndefined} from 'util';
import {ScoringStatus} from '../../services/scoring-status.enum';
import {OfferStatus} from '../../api/scoring-offer/offer-status.enum';
import {ScoringResponse} from '../../api/scoring/scoring-response';

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
  public scoringCompletenessInPercents;

  public ScoringStatus = ScoringStatus;

  constructor(private projectApiClient: ProjectApiClient,
              private router: Router,
              private route: ActivatedRoute,
              private userContext: UserContext) {

    route.params.subscribe(val => {
      this.reloadProjectAsync();
    });
  }

  public async ngOnInit() {
    await this.reloadProjectAsync();
  }

  private async reloadProjectAsync() {
    const newProjectId = +this.route.snapshot.paramMap.get('id');
    if (!isNullOrUndefined(this.projectId) && this.projectId === newProjectId) {
      return;
    }
    this.projectId = newProjectId;
    const selectedTabName = this.route.snapshot.paramMap.get('tab');
    if (!isNullOrUndefined(selectedTabName) && this.tabItems.includes(selectedTabName)) {
      this.selectedTab = this.tabItems.indexOf(selectedTabName);
    }

    this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);

    if (this.project) {
      this.isProjectExists = true;

      if (this.project.scoring.scoringStatus === ScoringStatus.InProgress) {
        this.scoringCompletenessInPercents = this.getScoringCompleteness(this.project.scoring);
      }

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

  private getScoringCompleteness(scoring: ScoringResponse): number {
    const finishedOffers = scoring.offers.filter(o => o.status === OfferStatus.Finished).length;
    const totalOffers = scoring.requiredExpertsCount;
    return Math.round(finishedOffers * 100 / totalOffers);
  }
}
