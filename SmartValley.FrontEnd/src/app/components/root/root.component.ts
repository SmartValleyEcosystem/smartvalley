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
    const response = await this.projectApiClient.getScoredProjectsAsync();
    this.projects = [];
    for (const projectResponse of response.items) {
      this.projects.push(<Project>{
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
}
