import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {v4 as uuid} from 'uuid';
import {ProjectManagerContractClient} from '../../services/project-manager-contract-client';
import {ContractApiClient} from '../../api/contract/contract-api-client';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent {

  applicationForm: FormGroup;

  hidden = false;

  teamMembers: Array<FormGroup>;

  constructor(private formBuilder: FormBuilder,
              private authenticationService: AuthenticationService,
              private applicationApiClient: ApplicationApiClient,
              private contractApiClient: ContractApiClient,
              private projectManagerContractClient: ProjectManagerContractClient) {
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
    application.projectArea = formModel.area;
    application.projectStatus = formModel.projectStatus;
    application.softCap = formModel.softCap;
    application.solutionDescription = formModel.solutionDescription;
    application.whitePaperLink = formModel.whitePaperLink;

    application.projectId = uuid();

    const user = await this.authenticationService.getCurrentUser();
    application.authorAddress = user.account;
    application.teamMembers = [];
    for (const teamMember of this.teamMembers) {
      application.teamMembers.push(teamMember.value);
    }

    const projectManagerContract = await this.contractApiClient.getProjectManagerContractAsync();
    application.transactionHash = await this.projectManagerContractClient.addProjectAsync(
      projectManagerContract.address,
      projectManagerContract.abi,
      application.projectId,
      formModel.name);

    return application;
  }

  createForm() {
    this.applicationForm = this.formBuilder.group({
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

        const group = this.formBuilder.group({
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

  changeHidden() {
    this.hidden = true;
  }

  async onSubmit() {
    const application = await this.FillApplication();
    await this.applicationApiClient.createApplicationAsync(application);
  }
}
