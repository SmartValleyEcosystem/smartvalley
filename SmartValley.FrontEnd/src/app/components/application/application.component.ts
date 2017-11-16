import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent {

  applicationForm: FormGroup;

  teamMembers: Array<FormGroup>;

  constructor(private fb: FormBuilder, private authenticationService: AuthenticationService, private apiClient: ApplicationApiClient) {
    this.createForm();
  }

  private async FillApplication(): Promise<Application> {

    const formModel = this.applicationForm.value;

    const application = new Application();
    application.attractedInvestments = formModel.attractedInvestments;
    application.blockChainType = formModel.blockChainType;
    application.country = formModel.country;
    application.financeModelLink = formModel.financeModelLink;
    application.hardCap = formModel.hardCap;
    application.mvpLink = formModel.mvpLink;
    application.name = formModel.name;
    application.probablyDescription = formModel.probablyDescription;
    application.projectArea = formModel.projectArea;
    application.projectStatus = formModel.projectStatus;
    application.softCap = formModel.softCap;
    application.solutionDescription = formModel.solutionDescription;
    application.whitePaperLink = formModel.whitePaperLink;

    const userInfo = await this.authenticationService.getUserInfo();
    application.authorAddress = userInfo.ethereumAddress;
    application.TeamMembers = [];
    for (const teamMember of this.teamMembers) {
      application.TeamMembers.push(teamMember.value);
    }

    return application;
  }

  createForm() {
    this.applicationForm = this.fb.group({
      name: ['', Validators.required],
      whitePaperLink: ['', Validators.pattern('https?://.+')],
      projectArea: '',
      probablyDescription: '',
      solutionDescription: '',
      projectStatus: '',
      softCap: 0.0,
      hardCap: 0.0,
      financeModelLink: ['', Validators.pattern('https?://.+')],
      country: '',
      attractedInvestments: false,
      blockChainType: '',
      mvpLink: ['', Validators.pattern('https?://.+')],
    });

    this.teamMembers = [];
    for (const item in EnumTeamMemberType) {
      if (typeof EnumTeamMemberType[item] === 'number') {

        const group = this.fb.group({
          memberType: EnumTeamMemberType[item],
          title: item,
          fullName: '',
          facebookLink: ['', Validators.pattern('https?://.+')],
          linkedInLink: ['', Validators.pattern('https?://.+')],
        });
        this.teamMembers.push(group);
      }
    }
  }

  async onSubmit() {
    const application = await this.FillApplication();
    await this.apiClient.createApplicationAsync(application);
  }
}
