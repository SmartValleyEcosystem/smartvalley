import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {Paths} from '../../paths';
import {AuthenticationService} from '../../services/authentication-service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {ProjectsForScoringRequest} from '../../api/scoring/projecs-for-scoring-request';


@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements AfterViewInit {

  public projectsForScoring: Array<Project>;
  public myProjects: Array<Project>;

  @ViewChild('projectsTabSet')
  private projectsTabSet: NgbTabset;

  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService,
              private route: ActivatedRoute,
              private router: Router) {
    this.loadProjectsForCategory(ScoringCategory.HR);
    this.loadMyProjects();
  }

  ngAfterViewInit(): void {
    this.route.queryParams.subscribe(params => {
      const value = params['tab'] || 'none';
      if (value === 'myProjects') {
        this.projectsTabSet.select('myProjects');
      }
    });
  }

  tabChanged($event: any) {
    let scoringCategory: ScoringCategory = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        scoringCategory = ScoringCategory.HR;
        break;
      case 1 :
        scoringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scoringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scoringCategory = ScoringCategory.TechnicalExpert;
        break;
    }
    this.loadProjectsForCategory(scoringCategory);
  }

  private async loadProjectsForCategory(scoringCategory: ScoringCategory) {
    const currentAccount = this.authenticationService.getCurrentUser().account;
    this.projectsForScoring = [];
    const projects = await this.scoringApiClient.getProjectForScoringAsync(<ProjectsForScoringRequest>{
      scoringCategory: <number>scoringCategory,
      expertAddress: currentAccount
    });
    for (const projectResponse of projects.items) {
      this.projectsForScoring.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        scoringCategory: scoringCategory,
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
}
