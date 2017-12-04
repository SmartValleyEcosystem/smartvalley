import {AfterViewInit, Component, OnDestroy, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {Paths} from '../../paths';
import {AuthenticationService} from '../../services/authentication-service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ProjectsForScoringRequest} from '../../api/scoring/projecs-for-scoring-request';
import {Constants} from '../../constants';
import {isNullOrUndefined} from 'util';
import {Subscription} from 'rxjs/Subscription';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements AfterViewInit, OnDestroy {
  public projectsForScoring: Array<Project>;
  public myProjects: Array<Project>;

  @ViewChild('projectsTabSet')
  private projectsTabSet: NgbTabset;
  private knownTabs = [Constants.ScoringMyProjectsTab, Constants.ScoringProjectsForScoringTab];
  private accountChangedSubscription: Subscription;

  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService,
              private activatedRoute: ActivatedRoute,
              private router: Router) {
    this.loadData();
    this.accountChangedSubscription = this.authenticationService.accountChanged.subscribe(() => this.loadData());
  }

  private loadData() {
    this.loadProjectsForCategory(ExpertiseArea.HR);
    this.loadMyProjects();
  }

  ngAfterViewInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const tab = params[Constants.TabQueryParam];
      if (this.knownTabs.includes(tab)) {
        this.projectsTabSet.select(tab);
      }
    });
  }

  onExpertiseTabChanged($event: any) {
    let expertiseArea: ExpertiseArea = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        expertiseArea = ExpertiseArea.HR;
        break;
      case 1 :
        expertiseArea = ExpertiseArea.Lawyer;
        break;
      case 2 :
        expertiseArea = ExpertiseArea.Analyst;
        break;
      case 3 :
        expertiseArea = ExpertiseArea.TechnicalExpert;
        break;
    }
    this.loadProjectsForCategory(expertiseArea);
  }

  onMainTabChanged($event: any) {
    const queryParams = Object.assign({}, this.activatedRoute.snapshot.queryParams);
    queryParams[Constants.TabQueryParam] = $event.nextId;
    this.router.navigate([Paths.Scoring], {queryParams: queryParams, replaceUrl: true});
  }

  private async loadProjectsForCategory(expertiseArea: ExpertiseArea) {
    this.projectsForScoring = [];
    const projects = await this.scoringApiClient.getProjectForScoringAsync(<ProjectsForScoringRequest>{
      expertiseArea: <number>expertiseArea
    });
    for (const projectResponse of projects.items) {
      this.projectsForScoring.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        expertiseArea: expertiseArea,
        address: projectResponse.address
      });
    }
  }

  private async loadMyProjects() {
    this.myProjects = [];
    const response = await this.scoringApiClient.getMyProjectsAsync();
    for (const projectResponse of response.items) {
      this.myProjects.push(<Project>{
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

  async createProject() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Application]);
    }
  }

  ngOnDestroy(): void {
    if (!isNullOrUndefined(this.accountChangedSubscription) && !this.accountChangedSubscription.closed) {
      this.accountChangedSubscription.unsubscribe();
    }
  }
}
