import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ScoredProject} from '../../api/expert/scored-project';
import {roundNumberPipe} from '../../utils/round-number.pipe';
import {Constants} from '../../constants';

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
              private projectApiClient: ProjectApiClient) {
  }

  public async ngOnInit() {
    this.projectsLink = Paths.ProjectList;
    let projectResponse = await this.projectApiClient.getScoredProjectAsync(0, 10);
    this.scoredProjects = projectResponse.items;
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Report + '/' + id]).toString()
    );
  }
}
