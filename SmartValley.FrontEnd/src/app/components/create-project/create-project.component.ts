import {Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {CreateProjectRequest} from '../../api/project/create-project-request';
import {SocialMediaTypeEnum} from '../../services/project/social-media-type.enum';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {isNullOrUndefined} from 'util';
import * as moment from 'moment';
import {TeamMemberRequest} from '../../api/project/team-member-request';
import {UpdateProjectRequest} from '../../api/project/update-project-request';
import {DialogService} from '../../services/dialog-service';
import {Paths} from '../../paths';
import {ErrorCode} from '../../shared/error-code.enum';
import {MyProjectResponse} from '../../api/project/my-project-response';
import {ProjectService} from '../../services/project/project.service';
import {TeamMemberResponse} from '../../api/project/team-member-response';
import {MemberUploadPhotoComponent} from '../member-upload-photo/member-upload-photo.component';
import {StringExtensions} from '../../utils/string-extensions';
import {ImageUploaderComponent} from '../image-uploader/image-uploader.component';

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
  public selectedSocials: number[] = [];
  public selectedMembers: number[] = [];
  public currentMemberId = 0;
  public membersGroup: FormGroup;
  public socialFormGroup: FormGroup;
  public isEditProjectRoute = false;
  public isSubmitDisable = false;

  public isEditing = false;
  public editingData: MyProjectResponse;
  private projectId: number;

  @ViewChild('name') public nameRow: ElementRef;
  @ViewChild('category') public categoryRow: ElementRef;
  @ViewChild('stage') public stageRow: ElementRef;
  @ViewChild('country') public countryRow: ElementRef;
  @ViewChild('description') public descriptionRow: ElementRef;
  @ViewChildren('required') public requiredFields: QueryList<any>;
  @ViewChildren('photo') photos: QueryList<MemberUploadPhotoComponent>;
  @ViewChild('projectImage') projectImage: ImageUploaderComponent;

  constructor(private formBuilder: FormBuilder,
              private notificationsService: NotificationsService,
              private projectApiClient: ProjectApiClient,
              private projectService: ProjectService,
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
    this.createForm();
    this.isEditProjectRoute = this.router.url === '/' + Paths.ProjectEdit;

    const memberGroupFields = {
      ['id__' + this.selectedMembers[this.selectedMembers.length - 1]]: '',
      ['full-name__' + this.selectedMembers[this.selectedMembers.length - 1]]: ['', [Validators.maxLength(200)]],
      ['role__' + this.selectedMembers[this.selectedMembers.length - 1]]: ['', [Validators.maxLength(100)]],
      ['linkedin__' + this.selectedMembers[this.selectedMembers.length - 1]]: ['', [Validators.maxLength(200),
        Validators.pattern('https?://.+')]],
      ['facebook__' + this.selectedMembers[this.selectedMembers.length - 1]]: ['', [Validators.maxLength(200),
        Validators.pattern('https?://.+')]],
      ['description__' + this.selectedMembers[this.selectedMembers.length - 1]]: ['', [Validators.maxLength(500)]],
    };

    this.membersGroup = this.formBuilder.group(memberGroupFields);

    const socialGroupFields = {
      ['social__1']: '',
      ['social-link__1']: ['', [Validators.maxLength(200), Validators.pattern('https?://.+')]]
    };
    this.socialFormGroup = this.formBuilder.group(socialGroupFields);

    await this.loadMyProjectDataAsync();
  }

  public addSocialMedia() {
    this.selectedSocials.push(this.selectedSocials.length + 1);

    this.socialFormGroup.addControl('social__' + this.selectedSocials[this.selectedSocials.length - 1],
      new FormControl(''));
    this.socialFormGroup.addControl('social-link__' + this.selectedSocials[this.selectedSocials.length - 1],
      new FormControl('', [Validators.maxLength(200), Validators.pattern('https?://.+')]));
  }

  public setSocialMedia(network?: number, value?: string) {
    this.socialFormGroup.controls['social__' + this.selectedSocials[this.selectedSocials.length - 1]].setValue(network);
    this.socialFormGroup.controls['social-link__' + this.selectedSocials[this.selectedSocials.length - 1]].setValue(value);
  }

  public removeSocialMedia(id: number) {
    this.selectedSocials = this.selectedSocials.filter(a => a !== id);
  }

  public removeTeamMember(id: number) {
    this.currentMemberId--;
    this.selectedMembers = this.selectedMembers.filter(a => a !== id);
  }

  public addTeamMember() {
    const newTeamMemberNumber = this.selectedMembers.length === 0 ? 1 : this.selectedMembers[this.selectedMembers.length - 1] + 1;
    this.selectedMembers.push(newTeamMemberNumber);

    this.membersGroup.addControl('id__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl(''));
    this.membersGroup.addControl('photo__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl(''));
    this.membersGroup.addControl('full-name__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl('',
      [Validators.maxLength(200)]));
    this.membersGroup.addControl('role__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl('',
      [Validators.maxLength(100)]));
    this.membersGroup.addControl('linkedin__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl('',
      [Validators.maxLength(200), Validators.pattern('https?://.+')]));
    this.membersGroup.addControl('facebook__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl('',
      [Validators.maxLength(200), Validators.pattern('https?://.+')]));
    this.membersGroup.addControl('description__' + this.selectedMembers[this.selectedMembers.length - 1], new FormControl('',
      [Validators.maxLength(500)]));
  }

  public async submitAsync() {
    if (!this.validateForm()) {
      return;
    }

    this.isSubmitDisable = true;
    this.isEditing ?
      await this.updateProjectAsync() :
      await this.createProjectAsync();
    this.isSubmitDisable = false;
  }

  private getTeamMemberRequests(): TeamMemberRequest[] {
    return this.selectedMembers.map(m => <TeamMemberRequest> {
      id: this.membersGroup.value['id__' + m] === '' ? 0 : this.membersGroup.value['id__' + m],
      fullName: this.membersGroup.value['full-name__' + m],
      role: this.membersGroup.value['role__' + m],
      about: this.membersGroup.value['description__' + m],
      facebook: StringExtensions.nullIfEmpty(this.membersGroup.value['facebook__' + m]),
      linkedin: StringExtensions.nullIfEmpty(this.membersGroup.value['linkedin__' + m])
    });
  }

  public setTeamMember(id ?: number, name ?: string, role ?: string, linkedin ?: string, facebook ?: string, description ?: string, photoUrl?: string) {
    this.membersGroup.controls['id__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(id);
    this.membersGroup.controls['full-name__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(name);
    this.membersGroup.controls['role__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(role);
    this.membersGroup.controls['linkedin__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(linkedin);
    this.membersGroup.controls['facebook__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(facebook);
    this.membersGroup.controls['description__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(description);
    this.membersGroup.controls['photo__' + this.selectedMembers[this.selectedMembers.length - 1]].setValue(photoUrl);
  }

  private getSocialNetworkLink(socialsArray: any, socialNetwork: string): string {
    const item = socialsArray.find(s => !isNullOrUndefined(s.network) && s.network.toLowerCase() === socialNetwork.toLowerCase());
    if (isNullOrUndefined(item)) {
      return null;
    }
    return StringExtensions.nullIfEmpty(item.link);
  }

  private createForm() {
    this.applicationForm = this.formBuilder.group({
      id: [0],
      name: ['', [Validators.required, Validators.maxLength(150)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      website: ['', [Validators.maxLength(200), Validators.pattern('https?://.+')]],
      whitePaperLink: ['', [Validators.maxLength(200), Validators.pattern('https?://.+')]],
      contactEmail: ['', Validators.maxLength(200)],
      projectImage: [''],
      icoDate: [''],
      category: ['', [Validators.required]],
      stage: ['', [Validators.required]],
      country: ['', [Validators.required]],
      social: [this.socials.first().value]
    });
  }

  private getCreateProjectRequest(): CreateProjectRequest {

    const socialsArray = this.selectedSocials.map(i => <any>{
      network: SocialMediaTypeEnum[this.socialFormGroup.value['social__' + i]],
      link: this.socialFormGroup.value['social-link__' + i]
    });

    const form = this.applicationForm.value;

    return <CreateProjectRequest>{
      contactEmail: StringExtensions.nullIfEmpty(form.contactEmail),
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      stage: form.stage,
      category: form.category,
      whitePaperLink: StringExtensions.nullIfEmpty(form.whitePaperLink),
      website: StringExtensions.nullIfEmpty(form.website),
      teamMembers: this.getTeamMemberRequests(),
      bitcointalk: this.getSocialNetworkLink(socialsArray, 'bitcointalk'),
      facebook: this.getSocialNetworkLink(socialsArray, 'facebook'),
      github: this.getSocialNetworkLink(socialsArray, 'github'),
      linkedin: this.getSocialNetworkLink(socialsArray, 'linkedIn'),
      medium: this.getSocialNetworkLink(socialsArray, 'medium'),
      reddit: this.getSocialNetworkLink(socialsArray, 'reddit'),
      telegram: this.getSocialNetworkLink(socialsArray, 'telegram'),
      twitter: this.getSocialNetworkLink(socialsArray, 'twitter')
    };
  }

  private getUpdateProjectRequest(): UpdateProjectRequest {
    const socialsArray = this.selectedSocials.map(i => <any>{
      network: SocialMediaTypeEnum[this.socialFormGroup.value['social__' + i]],
      link: this.socialFormGroup.value['social-link__' + i]
    });
    const form = this.applicationForm.value;
    return <UpdateProjectRequest>{
      id: form.id,
      contactEmail: StringExtensions.nullIfEmpty(form.contactEmail),
      countryCode: form.country,
      name: form.name,
      description: form.description,
      icoDate: form.icoDate,
      stage: form.stage,
      category: form.category,
      whitePaperLink: StringExtensions.nullIfEmpty(form.whitePaperLink),
      website: StringExtensions.nullIfEmpty(form.website),
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
    this.editingData = data;
    this.projectId = data.id;
    this.isEditing = true;
    const enumItems = Object.keys(SocialMediaTypeEnum)
      .filter(value => isNaN(+value));

    for (let i = 0; i < enumItems.length; i++) {
      const property = Object.entries(data).find(c => c.first().toLowerCase() === enumItems[i].toLowerCase());
      if (isNullOrUndefined(property)) {
        continue;
      }
      const link = property[1];
      if (!isNullOrUndefined(link)) {
        this.addSocialMedia();
        this.setSocialMedia(i, link);
      }
    }

    for (const t of data.teamMembers) {
      this.addTeamMember();
      this.setTeamMember(
        t.id,
        t.fullName,
        t.role,
        t.linkedIn,
        t.facebook,
        t.about,
        t.photoUrl);
      if (data.teamMembers.indexOf(t) === data.teamMembers.length - 1) {
        break;
      }
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
      projectImage: data.imageUrl,
      icoDate: isNullOrUndefined(data.icoDate) ? null : moment(data.icoDate).toDate(),
      contactEmail: data.contactEmail,
      social: [this.socials.first().value]
    });
  }

  private async updateProjectAsync(): Promise<void> {
    const request = this.getUpdateProjectRequest();

    const response = await this.projectService.updateAsync(request);

    await Promise.all([this.sendProjectImageAsync(response.projectId), this.sendPhotosAsync(response.teamMembers)]);

    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('CreateProject.ProjectUpdated')
    );

    const projectResponse = await this.projectApiClient.getMyProjectAsync();
    await this.router.navigate([Paths.Project + '/' + projectResponse.id]);
  }

  private async createProjectAsync(): Promise<void> {
    const request = this.getCreateProjectRequest();

    const response = await this.projectService.createAsync(request);

    await Promise.all([this.sendProjectImageAsync(response.projectId), this.sendPhotosAsync(response.teamMembers)]);

    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('CreateProject.ProjectCreated')
    );

    const myProjectIdResponse = await this.projectApiClient.getMyProjectAsync();
    await this.router.navigate([Paths.Project + '/' + myProjectIdResponse.id]);
  }

  private async sendProjectImageAsync(projectId: number): Promise<void> {
    if (this.projectImage.imgUrl) {
      if (this.projectImage.value) {
        await this.projectService.uploadProjectImageAsync(projectId, this.projectImage.value);
      }
    } else {
      await this.projectApiClient.deleteProjectImageAsync(projectId);
    }
  }

  private async sendPhotosAsync(teamMembers: TeamMemberResponse[]): Promise<void> {
    for (let i = 0; i < teamMembers.length; i++) {
      const memberNumber = i + 1;
      const photo = this.photos.find(p => p.elementId === 'photo__' + memberNumber);
      if (photo.imgUrl) {
        if (photo.value) {
          await this.projectService.uploadTeamMemberPhotoAsync(teamMembers[i].id.toString(), photo.value);
        }
      } else {
        await this.projectApiClient.deleteTeamMemberPhotoAsync(teamMembers[i].id);
      }
    }
  }

  public async deleteProjectAsync(): Promise<void> {
    const shouldDelete = await this.dialogService.showDeleteProjectModalAsync();
    if (shouldDelete) {
      try {
        await this.projectService.deleteAsync(this.projectId);
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
