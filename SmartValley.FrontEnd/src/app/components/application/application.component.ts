import { Component } from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {Application} from '../../services/application';


@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent {

  applicationForm: FormGroup;

  private application: Application;

  constructor(private fb: FormBuilder, private authenticationService: AuthenticationService, private apiClient: ApplicationApiClient) {
    this.createForm();
  }

  private async FillApplication() {
    this.application = new Application();
    const userInfo = await this.authenticationService.getUserInfo();

    const formModel = this.applicationForm.value;

    this.application.attractInv = formModel.attractInv;
    this.application.blockChainType = formModel.blockChainType;
    this.application.CEO = formModel.CEO;
    this.application.CFO = formModel.CFO;
    this.application.CMO = formModel.CMO;
    this.application.country = formModel.country;
    this.application.CTO = formModel.CTO;
    this.application.finModelLink = formModel.finModelLink;
    this.application.hardCap = formModel.hardCap;
    this.application.mvpLink = formModel.mvpLink;
    this.application.name = formModel.name;
    this.application.PR = formModel.PR;
    this.application.probDesc = formModel.probDesc;
    this.application.projectArea = formModel.projectArea;
    this.application.projStat = formModel.projStat;
    this.application.softCap = formModel.softCap;
    this.application.solDesc = formModel.solDesc;
    this.application.wpLink = formModel.wpLink;

    this.application.authorAddress = userInfo.ethereumAddress;
  }

  createForm() {
    this.applicationForm = this.fb.group({
      name: ['', Validators.required],
      wpLink: ['', Validators.pattern('https?://.+')],
      projectArea: '',
      probDesc: '',
      solDesc: '',
      projStat: '',
      softCap: '',
      hardCap: '',
      finModelLink: ['', Validators.pattern('https?://.+')],
      country: '',
      attractInv: '',
      blockChainType: '',
      mvpLink: ['', Validators.pattern('https?://.+')],
      CEO: this.fb.group({
        fullName: '',
        fbLink: '',
        linkedInLink: ['', Validators.pattern('https?://.+')],
      }),
      CTO: this.fb.group({
        fullName: '',
        fbLink: '',
        linkedInLink: ['', Validators.pattern('https?://.+')],
      }),
      CFO: this.fb.group({
        fullName: '',
        fbLink: '',
        linkedInLink: ['', Validators.pattern('https?://.+')],
      }),
      CMO: this.fb.group({
        fullName: '',
        fbLink: '',
        linkedInLink: ['', Validators.pattern('https?://.+')],
      }),
      PR: this.fb.group({
        fullName: '',
        fbLink: '',
        linkedInLink: ['', Validators.pattern('https?://.+')],
      })
    });
  }

  async onSubmit() {
    await this.FillApplication();
    await this.apiClient.createProjectAsync(this.application);
  }
}




