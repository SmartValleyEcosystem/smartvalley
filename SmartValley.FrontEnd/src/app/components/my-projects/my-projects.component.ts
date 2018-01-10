import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {Paths} from '../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.css']
})
export class MyProjectsComponent implements OnInit {

  public projects: Array<Project> = [];

  constructor(private scoringApiClient: ScoringApiClient,
              private router: Router) {
  }

  ngOnInit() {
    this.loadProjectsAsync();
  }

  private async loadProjectsAsync(): Promise<void> {
    const response = await this.scoringApiClient.getMyProjectsAsync();
    this.projects = response.items.map(p => Project.create(p));
  }

  public async navigateToApplicationPageAsync(): Promise<void> {
    await this.router.navigate([Paths.Application]);
  }
}
