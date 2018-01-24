import {Component, OnInit} from '@angular/core';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {ProjectCardType} from '../../services/project-card-type';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectCardData} from '../common/project-card/project-card-data';

@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.css']
})
export class MyProjectsComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public projects: Array<ProjectCardData> = [];

  constructor(private projectApiClient: ProjectApiClient,
              private router: Router,
              private authenticationService: AuthenticationService) {
    this.authenticationService.accountChanged.subscribe(async () => {
      await this.loadProjectsAsync();
    });
  }

  async ngOnInit() {
    await this.loadProjectsAsync();
  }

  private async loadProjectsAsync(): Promise<void> {
    const response = await this.projectApiClient.getMyProjectsAsync();
    this.projects = response.items.map(p => ProjectCardData.fromMyProjectsItemResponse(p));
  }

  public async navigateToApplicationPageAsync(): Promise<void> {
    await this.router.navigate([Paths.Application]);
  }
}
