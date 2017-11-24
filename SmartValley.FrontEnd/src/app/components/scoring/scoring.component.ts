import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {Paths} from '../../paths';
import {AuthenticationService} from '../../services/authentication-service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {EnumExpertType} from '../../services/enumExpertType';


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
    this.loadProjectsForCategory(EnumExpertType.HR);
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
    let scoringCategory: EnumExpertType = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        scoringCategory = EnumExpertType.HR;
        break;
      case 1 :
        scoringCategory = EnumExpertType.Lawyer;
        break;
      case 2 :
        scoringCategory = EnumExpertType.Analyst;
        break;
      case 3 :
        scoringCategory = EnumExpertType.TechnicalExpert;
        break;
    }
    this.loadProjectsForCategory(scoringCategory);
  }

  private async loadProjectsForCategory(scoringCategory: EnumExpertType) {
    this.projectsForScoring = [];
    const projects = await this.scoringApiClient.getProjectForScoringAsync({scoringCategory: <number>scoringCategory});
    for (const projectResponse of projects.items) {
      this.projectsForScoring.push(<Project>{
        id: projectResponse.id,
        name: projectResponse.name,
        area: projectResponse.area,
        country: projectResponse.country,
        score: projectResponse.score,
        description: projectResponse.description,
        imgUrl: 'https://png.icons8.com/?id=50284&size=280',
        scoringCategory: scoringCategory
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
