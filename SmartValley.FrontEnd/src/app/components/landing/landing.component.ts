import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {VotingService} from '../../services/voting/voting-service';

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
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.initializeProjectsCollection();
    this.canVote = await this.sprintService.hasActiveSprintAsync();
  }

  async navigateToVoting() {
    await this.router.navigate([Paths.Voting]);
  }

  async navigateToScoring() {
    await this.router.navigate([Paths.Scoring]);
  }

  async createProject() {
    await this.router.navigate([Paths.Application]);
  }

  private async initializeProjectsCollection() {
    const response = await this.projectApiClient.getScoredProjectsAsync();
    this.scoredProjects = response.items.map(p => ProjectCardData.fromProjectResponse(p));
  }
}
