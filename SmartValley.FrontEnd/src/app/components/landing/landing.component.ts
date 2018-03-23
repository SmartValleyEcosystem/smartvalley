import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ScoredProject} from '../../api/expert/scored-project';
import {ProjectFilter} from '../../api/project/project-filter';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';

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
    const projectResponse = await this.projectApiClient.getFilteredProjectsAsync(<ProjectFilter>{
      offset: 0,
      count: 10,
      orderBy: ProjectsOrderBy.ScoringEndDate
    });
    this.scoredProjects = projectResponse.items;
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Report + '/' + id]).toString()
    );
  }
}
