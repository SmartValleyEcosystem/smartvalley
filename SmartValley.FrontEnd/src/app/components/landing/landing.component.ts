import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {VotingService} from '../../services/voting/voting-service';
import {AuthenticationService} from '../../services/authentication/authentication-service';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  public scoredProjects: Array<ProjectCardData>;
  public ProjectCardType = ProjectCardType;
  public canVote: boolean;

  constructor(private router: Router,
              private projectApiClient: ProjectApiClient,
              private sprintService: VotingService,
              private authenticationService: AuthenticationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.initializeProjectsCollection();
  }

  async navigateToVoting() {
    await this.router.navigate([Paths.Voting]);
  }

  async navigateToScoring() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Scoring]);
    }
  }

  async createProject() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Application]);
    }
  }

  private async initializeProjectsCollection() {
    const response = await this.projectApiClient.getScoredProjectsAsync();
    this.canVote = await this.sprintService.hasActiveSprintAsync();
    this.scoredProjects = response.items.map(p => ProjectCardData.fromProjectResponse(p));
  }
}
