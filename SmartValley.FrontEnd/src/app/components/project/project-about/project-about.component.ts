import {Component, Inject, Input, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectAboutResponse} from '../../../api/project/project-about-response';
import {isNullOrUndefined} from 'util';
import {ProjectComponent} from '../project.component';

@Component({
  selector: 'app-project-about',
  templateUrl: './project-about.component.html',
  styleUrls: ['./project-about.component.css']
})
export class ProjectAboutComponent implements OnInit {

  public projectInfo: ProjectAboutResponse;
  public haveSocials: boolean;
  @Input() projectId: number;

  constructor(@Inject(ProjectComponent) public parent: ProjectComponent,
              private projectApiClient: ProjectApiClient) {
  }

  public async ngOnInit() {
    this.projectInfo = await this.projectApiClient.getProjectAboutAsync(this.projectId);
    this.haveSocials = this.checkSocials();
  }

  private checkSocials(): boolean {
    return !isNullOrUndefined(this.projectInfo.bitcoinTalk) ||
      !isNullOrUndefined(this.projectInfo.facebook) ||
      !isNullOrUndefined(this.projectInfo.github) ||
      !isNullOrUndefined(this.projectInfo.linkedin) ||
      !isNullOrUndefined(this.projectInfo.medium) ||
      !isNullOrUndefined(this.projectInfo.reddit) ||
      !isNullOrUndefined(this.projectInfo.telegram) ||
      !isNullOrUndefined(this.projectInfo.twitter)
      ;
  }
}
