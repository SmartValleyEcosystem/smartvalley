import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Project} from '../../services/project';
import {ProjectApiClient} from '../../api/project/project-api-client';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent {

  public projects: Array<Project>;

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              private projectApiClient: ProjectApiClient) {

    this.initializeProjectsCollection();
  }

  async navigateToScoring() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring]);
    }
  }

  async createProject() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Application]);
    }
  }

  private async initializeProjectsCollection() {
    const projectResponses = await this.projectApiClient.getScoredProjectsAsync();
    this.projects = [];
    for (const response of projectResponses) {
      this.projects.push(<Project>{
        projectName: response.name,
        projectArea: response.area,
        projectCountry: response.country,
        scoringRating: response.score,
        projectDescription: response.description,
        projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
      });
    }
  }

  colorOfProjectRate(rate: string): string {
    const r = parseInt(rate, 10);
    if (r > 80) {
      return 'high_rate';
    }
    if (r > 45) {
      return 'medium_rate';
    }
    return 'low_rate';
  }
}
