import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Project} from '../../services/project';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Constants} from '../../constants';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit{

  public projects: Array<Project>;

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              private projectApiClient: ProjectApiClient) {
  }

  ngOnInit(): void {
    this.initializeProjectsCollection();
  }

  async navigateToScoring() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringProjectsForScoringTab}});
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
        address: projectResponse.address
      });
    }
  }
}
