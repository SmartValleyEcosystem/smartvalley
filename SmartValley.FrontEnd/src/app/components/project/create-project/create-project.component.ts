import {Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {CreateProjectRequest} from '../../../api/project/create-project-request';
import {SocialMediaTypeEnum} from '../../../services/project/social-media-type.enum';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {DictionariesService} from '../../../services/common/dictionaries.service';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {v4 as uuid} from 'uuid';
import {isNullOrUndefined} from 'util';
import * as moment from 'moment';
import {TeamMemberRequest} from '../../../api/project/team-member-request';
import {UpdateProjectRequest} from '../../../api/project/update-project-request';
import {DialogService} from '../../../services/dialog-service';
import {Paths} from '../../../paths';
import {ErrorCode} from '../../../shared/error-code.enum';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  public applicationForm: FormGroup;
  public categories: SelectItem[];
  public stages: SelectItem[];
  public countries: SelectItem[];
  public socials: SelectItem[];
  public activeSocials: number[];
  public membersAmount: number[] = [];
  public currentMemberId = 1;
  public membersGroup: FormGroup;
  public socialFormGroup: FormGroup;

  public isEditing = false;
  private projectId: number;

  @ViewChild('name') public nameRow: ElementRef;
  @ViewChild('category') public categoryRow: ElementRef;
  @ViewChild('stage') public stageRow: ElementRef;
  @ViewChild('country') public countryRow: ElementRef;
  @ViewChild('description') public descriptionRow: ElementRef;
  @ViewChildren('required') public requiredFields: QueryList<any>;


  constructor(private formBuilder: FormBuilder,
              private notificationsService: NotificationsService,
              private projectApiClient: ProjectApiClient,
              private translateService: TranslateService,
              private dictionariesService: DictionariesService,
              private dialogService: DialogService,
              private router: Router) {

    this.categories = this.dictionariesService.categories.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });

    this.socials = this.dictionariesService.networks.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });

    this.stages = this.dictionariesService.stages.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });

    this.countries = this.dictionariesService.countries.map(i => <SelectItem>{
      label: i.name,
      value: i.code
    });
  }

  public async ngOnInit() {
    this.activeSocials = [1];
    this.membersAmount.push(this.currentMemberId);
    this.createForm();

    const memberGroupFields = {
      ['id__' + this.membersAmount[this.membersAmount.length - 1]]: '',
      ['full-name__' + this.membersAmount[this.membersAmount.length - 1]]: '',
      ['role__' + this.membersAmount[this.membersAmount.length - 1]]: '',
      ['linkedin__' + this.membersAmount[this.membersAmount.length - 1]]: '',
      ['facebook__' + this.membersAmount[this.membersAmount.length - 1]]: '',
      ['description__' + this.membersAmount[this.membersAmount.length - 1]]: '',
    };

    this.membersGroup = this.formBuilder.group(memberGroupFields);

    const socialGroupFields = {
      ['social__1']: '',
      ['social-link__1']: ''
    };
    this.socialFormGroup = this.formBuilder.group(socialGroupFields);

    await this.loadMyProjectDataAsync();
  }

  public addSocialMedia() {
    this.activeSocials.push(this.activeSocials.length + 1);

    this.socialFormGroup.addControl('social__' + this.activeSocials[this.activeSocials.length - 1], new FormControl(''));
    this.socialFormGroup.addControl('social-link__' + this.activeSocials[this.activeSocials.length - 1], new FormControl(''));
  }

  public setSocialMedia(network?: number, value?: string) {
    this.socialFormGroup.controls['social__' + this.activeSocials[this.activeSocials.length - 1]].setValue(network);
    this.socialFormGroup.controls['social-link__' + this.activeSocials[this.activeSocials.length - 1]].setValue(value);
  }

  public removeSocialMedia(id: number) {
    this.activeSocials = this.activeSocials.filter(a => a !== id);
  }

  public removeTeamMember(id: number) {
    this.currentMemberId--;
    this.membersAmount = this.membersAmount.filter(a => a !== id);
  }

  public addTeamMember() {
    this.currentMemberId++;
    this.membersAmount.push(this.currentMemberId);

    this.membersGroup.addControl('id__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
    this.membersGroup.addControl('full-name__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
    this.membersGroup.addControl('role__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
    this.membersGroup.addControl('linkedin__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
    this.membersGroup.addControl('facebook__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
    this.membersGroup.addControl('description__' + this.membersAmount[this.membersAmount.length - 1], new FormControl(''));
  }

  public async submitAsync() {
    if (!this.validateForm()) {
      return;
    }

    this.isEditing ?
      await this.updateProjectAsync() :
      await this.createProjectAsync();
  }

  private getTeamMemberRequests(): TeamMemberRequest[] {
    const teamMemberRequests: TeamMemberRequest[] = [];
    for (const i of this.membersAmount) {
      const currentMember = <TeamMemberRequest> {
        id: this.membersGroup.value['id__' + i] === '' ? 0 : this.membersGroup.value['id__' + i],
        fullName: this.membersGroup.value['full-name__' + i],
        role: this.membersGroup.value['role__' + i],
        about: this.membersGroup.value['description__' + i],
        facebook: this.membersGroup.value['facebook__' + i],
        linkedin: this.membersGroup.value['linkedin__' + i]
      };

      teamMemberRequests.push(currentMember);
    }
    return teamMemberRequests;
  }

  public setTeamMember(id?: number, name?: string, role?: string, linkedin?: string, facebook?: string, description?: string) {
    this.membersGroup.controls['id__' + this.membersAmount[this.membersAmount.length - 1]].setValue(id);
    this.membersGroup.controls['full-name__' + this.membersAmount[this.membersAmount.length - 1]].setValue(name);
    this.membersGroup.controls['role__' + this.membersAmount[this.membersAmount.length - 1]].setValue(role);
    this.membersGroup.controls['linkedin__' + this.membersAmount[this.membersAmount.length - 1]].setValue(linkedin);
    this.membersGroup.controls['facebook__' + this.membersAmount[this.membersAmount.length - 1]].setValue(facebook);
    this.membersGroup.controls['description__' + this.membersAmount[this.membersAmount.length - 1]].setValue(description);
  }

  private getSocialNetworkLink(socialsArray: any, socialNetwork: string): string {
    const item = socialsArray.find(s => !isNullOrUndefined(s.network) && s.network.toLowerCase() === socialNetwork);
    if (isNullOrUndefined(item)) {
      return null;
    }
    return item.link;
  }

  private createForm() {
    this.applicationForm = this.formBuilder.group({
      id: [0],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      website: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      whitePaperLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      contactEmail: ['', Validators.maxLength(100)],
      icoDate: [''],
      category: ['', [Validators.required]],
      stage: ['', [Validators.required]],
      country: ['', [Validators.required]],
      social: [this.socials[0].value]
    });
  }

  private getCreateProjectRequest(): CreateProjectRequest {
    const socialsArray = [];
    for (let i = 1; i <= this.activeSocials.length; i++) {
      const network = {
        network: SocialMediaTypeEnum[this.socialFormGroup.value['social__' + i]],
        link: this.socialFormGroup.value['social-link__' + i]
      };
      socialsArray.push(network);
    }

    const form = this.applicationForm.value;
    return <CreateProjectRequest>{
      externalId: uuid(),
      contactEmail: form.contactEmail,
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      stage: form.stage,
      category: form.category,
      whitePaperLink: form.whitePaperLink,
      projectId: uuid(),
      website: form.website,
      teamMembers: this.getTeamMemberRequests(),
      bitcointalk: this.getSocialNetworkLink(socialsArray, 'bitcointalk'),
      facebook: this.getSocialNetworkLink(socialsArray, 'facebook'),
      github: this.getSocialNetworkLink(socialsArray, 'github'),
      linkedin: this.getSocialNetworkLink(socialsArray, 'linkedIn'),
      medium: this.getSocialNetworkLink(socialsArray, 'medium'),
      reddit: this.getSocialNetworkLink(socialsArray, 'reddit'),
      telegram: this.getSocialNetworkLink(socialsArray, 'telegram'),
      twitter: this.getSocialNetworkLink(socialsArray, 'twitter'),
    };
  }

  private getUpdateProjectRequest(): UpdateProjectRequest {
    const socialsArray = [];
    for (let i = 1; i <= this.activeSocials.length; i++) {
      const network = {
        network: SocialMediaTypeEnum[this.socialFormGroup.value['social__' + i]],
        link: this.socialFormGroup.value['social-link__' + i]
      };
      socialsArray.push(network);
    }
    const form = this.applicationForm.value;
    return <UpdateProjectRequest>{
      id: form.id,
      contactEmail: form.contactEmail,
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      stage: form.stage,
      category: form.category,
      whitePaperLink: form.whitePaperLink,
      projectId: uuid(),
      website: form.website,
      teamMembers: this.getTeamMemberRequests(),
      bitcointalk: this.getSocialNetworkLink(socialsArray, 'bitcointalk'),
      facebook: this.getSocialNetworkLink(socialsArray, 'facebook'),
      github: this.getSocialNetworkLink(socialsArray, 'github'),
      linkedin: this.getSocialNetworkLink(socialsArray, 'linkedIn'),
      medium: this.getSocialNetworkLink(socialsArray, 'medium'),
      reddit: this.getSocialNetworkLink(socialsArray, 'reddit'),
      telegram: this.getSocialNetworkLink(socialsArray, 'telegram'),
      twitter: this.getSocialNetworkLink(socialsArray, 'twitter'),
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
    } else if (this.applicationForm.controls['category'].invalid) {
      this.scrollToElement(this.categoryRow);
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


  private async loadMyProjectDataAsync() {
    const data = await this.projectApiClient.getMyProjectAsync();
    if (data == null) {
      return;
    }
    this.projectId = data.id;
    this.isEditing = true;
    const enumItems = Object.keys(SocialMediaTypeEnum)
      .filter(value => isNaN(+value));

    for (let i = 0; i < enumItems.length; i++) {
      const property = Object.entries(data).find(c => c[0].toLowerCase() === enumItems[i].toLowerCase());
      if (isNullOrUndefined(property)) {
        continue;
      }
      const link = property[1];
      if (!isNullOrUndefined(link)) {
        this.setSocialMedia(i, link);
        this.addSocialMedia();
      }
    }

    for (const t of data.teamMembers) {
      this.setTeamMember(
        t.id,
        t.fullName,
        t.role,
        t.linkedIn,
        t.facebook,
        t.about);
      if (data.teamMembers.indexOf(t) === data.teamMembers.length - 1) {
        break;
      }
      this.addTeamMember();
    }

    this.applicationForm.setValue({
      id: data.id,
      name: data.name,
      category: data.category,
      stage: data.stage,
      country: data.countryCode,
      description: data.description,
      website: data.website,
      whitePaperLink: data.whitePaperLink,
      icoDate: moment(data.icoDate).toDate(),
      contactEmail: data.contactEmail,
      social: [this.socials[0].value]
    });
  }

  private async updateProjectAsync(): Promise<void> {
    const request = this.getUpdateProjectRequest();

    await this.projectApiClient.updateAsync(request);

    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('CreateProject.ProjectUpdated')
    );
  }

  private async createProjectAsync(): Promise<void> {
    const request = this.getCreateProjectRequest();

    await this.projectApiClient.createAsync(request);

    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('CreateProject.ProjectCreated')
    );
  }


  public async deleteProjectAsync(): Promise<void> {
    const shouldDelete = await this.dialogService.showDeleteProjectModalAsync();
    if (shouldDelete) {
      try {
        await this.projectApiClient.deleteAsync(this.projectId);
      } catch (e) {
        if (e.error.errorCode === ErrorCode.ProjectCouldntBeRemoved) {
          this.notificationsService.warn(
            this.translateService.instant('Common.Failed'),
            this.translateService.instant('CreateProject.ProjectCouldntbeRemoved')
          );
        }
      }
      await this.router.navigate([Paths.Root]);
    }
  }
}
