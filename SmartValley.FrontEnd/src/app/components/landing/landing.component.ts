import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Project} from '../../services/project';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {VotingService} from '../../services/voting/voting-service';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  public projects: Array<Project>;
  public ProjectCardType = ProjectCardType;
  public canVote: boolean;

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              private projectApiClient: ProjectApiClient,
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.initializeProjectsCollection();
    this.canVote = await this.sprintService.hasActiveSprintAsync();
  }

  async navigateToVoting() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Voting]);
    }
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
        address: projectResponse.address
      });
    }
  }
}
