import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {VotingService} from '../../services/voting/voting-service';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {ScoredProject} from '../../api/expert/scored-project';
import {roundNumberPipe} from '../../utils/round-number.pipe';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public scoredProjects: ScoredProject[];
  public projectsLink: string;

  constructor(private router: Router,
              private projectApiClient: ProjectApiClient,
              private sprintService: VotingService,
              private authenticationService: AuthenticationService) {
  }

  public async ngOnInit() {
    this.projectsLink = Paths.MyProjects;
    let projectResponse = await this.projectApiClient.getScoredProjectAsync(0, 10);
    this.scoredProjects = projectResponse.items;
  }

  public getProjectLink(id) {
    return this.router.createUrlTree([Paths.Report], {queryParams: {id: id}}).toString();
  }

  public async navigateToScoring() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Expert]);
    }
  }
}
