import {Component, ViewChild, ElementRef, ViewChildren, QueryList, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
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
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {SocialMediaTypeEnum} from '../../services/project/social-media-type.enum';
import {CommonService} from '../../services/common/common.service';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent implements OnInit {
  public applicationForm: FormGroup;
  public isProjectCreating: boolean;
  public categories: SelectItem[];
  public stages: SelectItem[];
  public countries: SelectItem[];
  public socials: SelectItem[];
  public activeSocials: number[];
  public socialMediaEnumLength: number;
  public members: number[] = [];
  public currentMemberId = 1;
  public membersGroup: FormGroup;
  public socialFormGroup: FormGroup;

  @ViewChild('name') public nameRow: ElementRef;
  @ViewChild('area') public areaRow: ElementRef;
  @ViewChild('stage') public stageRow: ElementRef;
  @ViewChild('country') public countryRow: ElementRef;
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
              private projectService: ProjectService,
              private applicationApiClient: ApplicationApiClient,
              private commonService: CommonService) {

    this.categories =  this.commonService.categories.map(i => <SelectItem>{
      label: i.name,
      value: i.type
    });

    this.socials = this.commonService.networks.map(i => <SelectItem>{
      label: i.name,
      value: i.socialMediaType
    });

    this.stages = this.commonService.stages.map(i => <SelectItem>{
      label: i.name,
      value: i.type
    });

    this.countries = this.commonService.countries.map(i => <SelectItem>{
      label: i.name,
      value: i.code
    });
  }

  public addSocialMedia() {
    const newSocialInputId = this.activeSocials.length + 1;
    this.activeSocials.push(newSocialInputId);
    this.addSocialControls();
  }

  public removeSocialMedia(id) {
    this.activeSocials = this.activeSocials.filter( a => a !== id);
  }

  public addTeamMember() {
    this.currentMemberId++;
    this.members.push(this.currentMemberId);
    this.addTeamMemberControls();
  }

  public ngOnInit(): void {
    this.socialMediaEnumLength = Object.keys(SocialMediaTypeEnum).length / 2;
    this.activeSocials = [1];
    this.members.push(this.currentMemberId);
    this.createForm();

    const memberGroupFields = {
      ['full-name__' + this.members[this.members.length - 1]]: '',
      ['role__' + this.members[this.members.length - 1]]: '',
      ['linkedin__' + this.members[this.members.length - 1]]: '',
      ['facebook__' + this.members[this.members.length - 1]]: '',
      ['description__' + this.members[this.members.length - 1]]: '',
    };

    this.membersGroup = this.formBuilder.group(memberGroupFields);

    const socialGroupFileds = {
      ['social__1']: '',
      ['social-link__1']: ''
    };

    this.socialFormGroup = this.formBuilder.group(socialGroupFileds);
  }

  public addTeamMemberControls() {
    this.membersGroup.addControl('full-name__' + this.members[this.members.length - 1], new FormControl(''));
    this.membersGroup.addControl('role__' + this.members[this.members.length - 1], new FormControl(''));
    this.membersGroup.addControl('linkedin__' + this.members[this.members.length - 1], new FormControl(''));
    this.membersGroup.addControl('facebook__' + this.members[this.members.length - 1], new FormControl(''));
    this.membersGroup.addControl('description__' + this.members[this.members.length - 1], new FormControl(''));
  }

  public addSocialControls() {
    this.socialFormGroup.addControl('social__' + this.activeSocials[this.activeSocials.length - 1], new FormControl(''));
    this.socialFormGroup.addControl('social-link__' + this.activeSocials[this.activeSocials.length - 1], new FormControl(''));
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

  public getTeamMembers() {
    let teamArray = [];
    for (let i = 1; i <= this.members.length; i++) {
      const currentMember = {
        fullName: this.membersGroup.value['full-name__' + i],
        role: this.membersGroup.value['role__' + i],
        about: this.membersGroup.value['description__' + i],
        socialMedias: []
      };
      teamArray.push(currentMember);
    }
    return teamArray;
  }

  public getSocials() {
    let socialsArray = [];
    for (let i = 1; i <= this.activeSocials.length; i++) {
      const currenstSocial = {
        fullName: this.socialFormGroup.value['social__' + i],
        role: this.socialFormGroup.value['social-link__' + i],
        about: this.socialFormGroup.value['social-link__' + i],
      };
      socialsArray.push(currenstSocial);
    }
    return socialsArray;
  }

  private createForm() {
    this.applicationForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      website: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      whitePaperLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      contactEmail: ['', Validators.maxLength(100)],
      icoDate: [''],
      projectArea: ['', [Validators.required]],
      stage: ['', [Validators.required]],
      country: ['', [Validators.required]],
      social: [this.socials[0].value]
    });
  }

  private createSubmitApplicationRequest(): CreateProjectRequest {
    const form = this.applicationForm.value;
    return <CreateProjectRequest>{
      externalId: uuid(),
      contactEmail: form.contactEmail,
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      stageId: form.stage,
      category: form.projectArea,
      whitePaperLink: form.whitePaperLink,
      projectId: uuid(),
      website: form.website,
      teamMembers: this.getTeamMembers(),
      socialMedias: this.getSocials()
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
    } else if (this.applicationForm.controls['stage'].invalid) {
      this.scrollToElement(this.stageRow);
    } else if (this.applicationForm.controls['country'].invalid) {
      this.scrollToElement(this.countryRow);
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
