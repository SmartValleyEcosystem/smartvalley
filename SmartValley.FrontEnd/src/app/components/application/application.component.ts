import {Component, ViewChild, ElementRef, ViewChildren, QueryList, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {v4 as uuid} from 'uuid';
import {ScoringManagerContractClient} from '../../services/contract-clients/scoring-manager-contract-client';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {NotificationsService} from 'angular2-notifications';
import {DialogService} from '../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {BalanceService} from '../../services/balance/balance.service';
import {VotingManagerContractClient} from '../../services/contract-clients/voting-manager-contract-client';
import {UserContext} from '../../services/authentication/user-context';
import {SelectItem} from 'primeng/api';
import {CreateProjectRequest} from '../../api/project/create-project-request';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectService} from '../../services/project/project-service';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent implements OnInit {
  public applicationForm: FormGroup;
  public isProjectCreating: boolean;
  projectAreas: SelectItem[];
  stages: SelectItem[];
  countries: SelectItem[];
  socials: SelectItem[];

  @ViewChild('name') public nameRow: ElementRef;
  @ViewChild('area') public areaRow: ElementRef;
  @ViewChild('description') public descriptionRow: ElementRef;
  @ViewChildren('required') public requiredFields: QueryList<any>;

  constructor(private formBuilder: FormBuilder,
              private userContext: UserContext,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private votingManagerContractClient: VotingManagerContractClient,
              private router: Router,
              private notificationsService: NotificationsService,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private translateService: TranslateService,
              private balanceService: BalanceService,
              private projectService: ProjectService) {

    this.projectAreas = this.projectService.getProjectAreas().map(i => <SelectItem>{
      label: i.name,
      value: i.projectAreaType
    });

    this.socials = this.projectService.getSocialMedias().map(i => <SelectItem>{
      label: i.name,
      value: i.socialMediaType
    });

    this.stages = this.projectService.getStages().map(i => <SelectItem>{
      label: i.name,
      value: i.stageType
    });

    this.countries = [
      {label: 'Select City', value: null},
      {label: 'New York', value: 'RU'},
      {label: 'Rome', value: 'UK'}
    ];
  }

  public ngOnInit(): void {
    this.createForm();
  }

  public async onSaveAsync() {
    if (!this.validateForm()) {
      return;
    }
    const request = this.createSubmitApplicationRequest();

    await this.projectApiClient.createAsync(request);
    await this.balanceService.updateBalanceAsync();

    await this.router.navigate([Paths.MyProjects]);
    this.notifyProjectCreated();
  }

  private notifyProjectCreated() {
    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('Application.ProjectCreated')
    );
  }

  private createForm() {
    this.applicationForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      website: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      whitePaperLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      contactEmail: ['', Validators.maxLength(100)],
      icoDate: ['', [Validators.required, Validators.maxLength(100)]],
      projectArea: [this.projectAreas[0].value],
      country: [this.countries[0].value],
      stage: [this.stages[0].value],
      social: [this.socials[0].value],
      teamMember: []
    });
  }

  private createSubmitApplicationRequest(): CreateProjectRequest {
    const user = this.userContext.getCurrentUser();
    const form = this.applicationForm.value;
    return <CreateProjectRequest>{
      contactEmail: form.contactEmail,
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      projectAreaId: form.projectArea,
      whitePaperLink: form.whitePaperLink,
      projectId: uuid(),
      authorAddress: user.account,
      website: form.website
      //teamMembers: form.teamMembers.filter(m => !isNullOrUndefined(m.fullName))
    };
  }

  private validateForm(): boolean {
    if (!this.applicationForm.invalid) {
      return true;
    }
    this.scrollToInvalidElement();
    return false;
  }

  private scrollToInvalidElement() {
    if (this.applicationForm.controls['name'].invalid) {
      this.scrollToElement(this.nameRow);
    } else if (this.applicationForm.controls['projectArea'].invalid) {
      this.scrollToElement(this.areaRow);
    } else if (this.applicationForm.controls['description'].invalid) {
      this.scrollToElement(this.descriptionRow);
    }
  }

  private scrollToElement(element: ElementRef) {
    const containerOffset = element.nativeElement.offsetTop;
    const fieldOffset = element.nativeElement.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
    element.nativeElement.children[1].classList.add('ng-invalid');
    element.nativeElement.children[1].classList.add('ng-dirty');
    element.nativeElement.children[1].classList.remove('ng-valid');
    const invalidElements = this.requiredFields.filter(i => i.nativeElement.classList.contains('ng-invalid'));
    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        invalidElements[a].nativeElement.classList.add('ng-invalid');
        invalidElements[a].nativeElement.classList.add('ng-dirty');
      }
    }
  }
}
