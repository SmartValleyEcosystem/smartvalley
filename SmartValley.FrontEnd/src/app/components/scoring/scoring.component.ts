import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {Paths} from '../../paths';
import {AuthenticationService} from '../../services/authentication-service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements AfterViewInit {

  public projectsForScorring: Array<Project>;
  public myProjects: Array<Project>;

  @ViewChild('projectsTabSet')
  private  projectsTabSet: NgbTabset;

  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService,
              private route: ActivatedRoute,
              private router: Router) {
    this.loadProjectsForCategory(ScoringCategory.Hr);
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
    let index: number = $event.index;
    switch (index) {
      case 0 :
        scoringCategory = ScoringCategory.Hr;
        break;
      case 1 :
        scoringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scoringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scoringCategory = ScoringCategory.Tech;
        break;
    }
    this.loadProjectsForCategory(scoringCategory);
  }


  private async loadProjectsForCategory(scroringCategory: ScoringCategory) {
    this.projectsForScorring = [];
    const projects = await this.scoringApiClient.getProjectForScoringAsync({scoringCategory: scroringCategory});
    for (const projectResponse of projects.items) {
      this.projectsForScorring.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        imgUrl: 'https://png.icons8.com/?id=50284&size=280'
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
        imgUrl: 'https://png.icons8.com/?id=50284&size=280'
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
