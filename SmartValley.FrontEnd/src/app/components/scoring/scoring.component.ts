import {Component} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {Paths} from '../../paths';
import {AuthenticationService} from '../../services/authentication-service';
import {Router} from '@angular/router';


@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public projectsForScorring: Array<Project>;
  public myProjects: Array<Project>;

  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService,
              private router: Router) {
    this.getProjectsForCategory(ScoringCategory.Hr);
    this.getMyProjects();
  }

  tabChanged($event: any) {
    let scroringCategory: ScoringCategory = 1;
    let index: number = $event.index;
    switch (index) {
      case 0 :
        scroringCategory = ScoringCategory.Hr;
        break;
      case 1 :
        scroringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scroringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scroringCategory = ScoringCategory.Tech;
        break;
    }
    this.getProjectsForCategory(scroringCategory);
  }


  private async getProjectsForCategory(scroringCategory: ScoringCategory) {
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

  private async getMyProjects() {
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
