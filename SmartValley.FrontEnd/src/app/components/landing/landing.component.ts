import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ScoredProject} from '../../api/expert/scored-project';
import {ProjectQuery} from '../../api/project/project-query';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {ProjectResponse} from '../../api/project/project-response';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public projects: ScoredProject[];
  public projectsLink: string;

  constructor(private router: Router,
              private projectApiClient: ProjectApiClient) {
  }

  public async ngOnInit() {
    this.projectsLink = Paths.ProjectList;
    const projectResponse = await this.projectApiClient.queryProjectsAsync(<ProjectQuery>{
      offset: 0,
      count: 10,
      onlyScored: false,
      orderBy: ProjectsOrderBy.ScoringEndDate
    });
    this.projects = projectResponse.items.map(p => this.createScoredProject(p));
  }

  private createScoredProject(response: ProjectResponse): ScoredProject {
    return <ScoredProject> {
      id: response.id,
      address: response.address,
      category: response.category,
      country: response.country,
      description: response.description,
      name: response.name,
      score: response.score,
      scoringEndDate: response.scoringEndDate
    };
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Report + '/' + id]).toString()
    );
  }
}
