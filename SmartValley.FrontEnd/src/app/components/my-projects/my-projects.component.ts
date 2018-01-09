import { Component, OnInit } from '@angular/core';
import {Project} from '../../services/project';
import {ProjectResponse} from '../../api/project/project-response';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication-service';

@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.css']
})
export class MyProjectsComponent implements OnInit {

  public projects: Array<Project> = [];
  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService,
              private router: Router) { }

  ngOnInit() {
    this.loadMyProjectsAsync();
  }

  private async loadMyProjectsAsync(): Promise<void> {
    const response = await this.scoringApiClient.getMyProjectsAsync();
    this.myProjects = response.items.map(p => this.createProject(p));
  }

  public async navigateToApplicationPageAsync(): Promise<void> {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Application]);
    }
  }

  private createProject(response: ProjectResponse, expertiseArea: ExpertiseArea = ExpertiseArea.HR): Project {
    return <Project>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      expertiseArea: expertiseArea
    };
  }

}
