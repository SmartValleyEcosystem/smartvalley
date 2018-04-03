import {Component, Input, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectAboutResponse} from '../../../api/project/project-about-response';

@Component({
  selector: 'app-project-about',
  templateUrl: './project-about.component.html',
  styleUrls: ['./project-about.component.css']
})
export class ProjectAboutComponent implements OnInit {

  public projectInfo: ProjectAboutResponse;
  @Input() projectId: number;

  constructor(private projectApiClient: ProjectApiClient) {
  }

  public async ngOnInit() {
    this.projectInfo = await this.projectApiClient.getProjectAboutAsync(this.projectId);
  }

}
