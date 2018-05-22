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

  public editorFormats = [
      'bold',
      'underline',
      'strike',
      'header',
      'italic',
      'list',
      'indent',
      'color',
      'align',
      'blockquote',
      'indent',
      'background'
  ];
  public editorOptions = {
      toolbar: {
          container:
              [
                  ['bold', 'italic', 'underline', 'strike'],
                  [{ 'header': 1 }, { 'header': 2 }],
                  [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                  [{ 'indent': '-1' }, { 'indent': '+1' }],
                  [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                  [{ 'color': [] }, { 'background': [] }],
                  [{ 'align': [] }],

              ]
      },
      clipboard: {
          matchVisual: false
      }
  };

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
