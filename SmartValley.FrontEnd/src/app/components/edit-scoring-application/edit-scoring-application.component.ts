import {Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ScoringApplicationResponse} from '../../api/scoring-application/scoring-application-response';
import {ScoringApplicationPartition} from '../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../api/scoring-application/question-control-type.enum';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../../services/project/social-media-type.enum';
import {TeamMember} from '../../api/scoring-application/team-member';
import {SaveScoringApplicationRequest} from '../../api/scoring-application/save-scoring-application-request';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {Answer} from '../../api/scoring-application/answer';
import {Adviser} from '../../api/scoring-application/adviser';
import {isNullOrUndefined} from 'util';
import {Paths} from '../../paths';
import * as moment from 'moment';
import {ProjectApplicationInfoResponse} from '../../api/scoring-application/project-application-info-response';
import {NotificationsService} from 'angular2-notifications';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {DialogService} from '../../services/dialog-service';

@Component({
  selector: 'app-edit-scoring-application',
  templateUrl: './edit-scoring-application.component.html',
  styleUrls: ['./edit-scoring-application.component.scss']
})
export class EditScoringApplicationComponent implements OnInit, OnDestroy {

  public projectId: number;
  public questions: ScoringApplicationResponse;
  public partitions: ScoringApplicationPartition[];
  public questionControlType = QuestionControlType;
  public questionTypeLine = QuestionControlType[0];
  public questionTypeMultiLine = QuestionControlType[1];
  public questionTypeComboBox = QuestionControlType[2];
  public questionTypeDateTime = QuestionControlType[3];
  public questionTypeCheckBox = QuestionControlType[4];
  public questionTypeUrl = QuestionControlType[5];
  public questionFormGroup: FormGroup;
  public activeSocials: number[] = [];
  public activeArticleLink: number[] = [];
  public activeTeamMembers: number[] = [];
  public activeAdvisers: number[] = [];
  public categories: SelectItem[];
  public stages: SelectItem[];
  public countries: SelectItem[];
  public socials: SelectItem[];
  public projectInfo: ProjectApplicationInfoResponse;
  public disabled = false;
  public comboboxValues: { [id: number]: SelectItem[] };
  public savedTime: Date;
  public activePartition = '';
  public questionsReady = false;
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

  private isPrivateProject: boolean;

  @ViewChild('socialsContainer') private socialsContainer: ElementRef;
  @ViewChild('membersContainer') private membersContainer: ElementRef;
  @ViewChildren('required') public requiredFields: QueryList<any>;

  private timer: NodeJS.Timer;

  constructor(private scoringApplicationApiClient: ScoringApplicationApiClient,
              private translateService: TranslateService,
              private notificationsService: NotificationsService,
              private dictionariesService: DictionariesService,
              private formBuilder: FormBuilder,
              private htmlElement: ElementRef,
              private route: ActivatedRoute,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private router: Router) {
  }

  async ngOnInit() {
    this.createCommonForm();
    this.categories = this.dictionariesService.categories.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });
    this.stages = this.dictionariesService.stages.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });
    this.socials = this.dictionariesService.networks.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });
    this.countries = this.dictionariesService.countries.map(i => <SelectItem>{
      label: this.translateService.instant(i.name),
      value: i.code
    });

    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.questions = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.projectId);
    this.partitions = this.questions.partitions;
    this.projectInfo = this.questions.projectInfo;
    const project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    this.isPrivateProject = project.isPrivate;
    this.comboboxValues = this.getQuestionSelectItems(this.partitions);

    this.addQuestionsFormControls(this.partitions);
    await this.loadScoringApplicationDataAsync();

    this.timer = <NodeJS.Timer>setInterval(async () => await this.saveDraftAsync(), 60000);
    this.activePartition = 'partition_common';
  }

  ngOnDestroy(): void {
    clearInterval(this.timer);
  }

  private async loadScoringApplicationDataAsync(): Promise<void> {
    if (this.questions) {
      this.loadEditedQuestion(this.questions.partitions);
    }

    const data = this.projectInfo;
    if (data == null) {
      return;
    }

    if (this.projectInfo.articles) {
      const articles = JSON.parse(this.projectInfo.articles);
      for (const article of articles) {
        this.addArticleLink();
        this.socialsGroup.controls['article-link_' + this.activeArticleLink[this.activeArticleLink.length - 1]].setValue(article);
      }
    }

    const enumItems = Object.keys(SocialMediaTypeEnum)
      .filter(value => isNaN(+value));
    for (let i = 0; i < enumItems.length; i++) {
      const property = Object.entries(data.socialNetworks).find(c => c[0].toLowerCase() === enumItems[i].toLowerCase());
      if (isNullOrUndefined(property)) {
        continue;
      }
      const link = property[1];
      if (!isNullOrUndefined(link)) {
        this.addSocialMedia();
        this.setSocialMedia(i, link);
      }
    }

    if (data.projectAdvisers) {
      for (const t of data.projectAdvisers) {
        this.addAdviser();
        this.setAdviser(
          t.reason,
          t.fullName,
          t.about,
          t.linkedInLink,
          t.facebookLink);
        if (data.projectAdvisers.indexOf(t) === data.projectAdvisers.length - 1) {
          break;
        }
      }
    }

    if (data.projectTeamMembers) {
      for (const t of data.projectTeamMembers) {
        this.addTeamMember();
        this.setTeamMember(
          t.id,
          t.fullName,
          t.role,
          t.linkedIn,
          t.facebook,
          t.about);
        if (data.projectTeamMembers.indexOf(t) === data.projectTeamMembers.length - 1) {
          break;
        }
      }
    }

    this.commonGroup.setValue({
      name: data.name,
      projectCategory: data.category,
      projectStage: data.stage,
      country: data.countryCode,
      description: data.description,
      website: data.webSite,
      linkToWP: data.whitePaperLink,
      icoDate: data.icoDate == null ? '' : moment(data.icoDate).toDate(),
      email: data.contactEmail
    });
  }

  get commonGroup(): FormGroup {
    return this.questionFormGroup.get('commonGroup') as FormGroup;
  }

  get questionsGroup(): FormGroup {
    return this.questionFormGroup.get('questionsGroup') as FormGroup;
  }

  get socialsGroup(): FormGroup {
    return this.questionFormGroup.get('socialsGroup') as FormGroup;
  }

  get teamGroup(): FormGroup {
    return this.questionFormGroup.get('teamGroup') as FormGroup;
  }

  get advisersGroup(): FormGroup {
    return this.questionFormGroup.get('advisersGroup') as FormGroup;
  }

  public createCommonForm(): void {
    const commonFormControls = {
      name: ['', [Validators.required, Validators.maxLength(50)]],
      country: ['', [Validators.required]],
      projectStage: ['', [Validators.required]],
      projectCategory: ['', [Validators.required]],
      icoDate: ['', [Validators.required]],
      website: ['', [Validators.required, Validators.maxLength(200), Validators.pattern('https?://.+')]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      linkToWP: ['', [Validators.required, Validators.maxLength(200), Validators.pattern('https?://.+')]],
      email: ['', [Validators.required, Validators.maxLength(200), Validators.pattern('\\w+@\\w+\\.\\w+')]]
    };
    this.questionFormGroup = this.formBuilder.group({
      commonGroup: this.formBuilder.group(commonFormControls),
      socialsGroup: this.formBuilder.group([]),
      questionsGroup: this.formBuilder.group([]),
      teamGroup: this.formBuilder.group([]),
      advisersGroup: this.formBuilder.group([])
    });
  }

  public scrollToPartition(elementId: string): void {
    this.activePartition = 'partition_' + elementId;
    const element = this.htmlElement.nativeElement.querySelector('#partition_' + elementId);
    const containerOffset = element.offsetTop;
    const fieldOffset = element.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
  }

  public addQuestionsFormControls(partitions: ScoringApplicationPartition[]): void {
    partitions.selectMany(partition => partition.questions)
      .map(question => {
        const control = new FormControl('');
        if (question.type === +QuestionControlType.Url) {
          control.setValidators([Validators.required, Validators.pattern('https?://.+')]);
        } else {
          control.setValidators(Validators.required);
        }
        this.questionsGroup.addControl('control_' + question.id, control);
      });

    this.questionsReady = true;
  }

  public getQuestionTypeById(id: number): string {
    return this.questionControlType[id];
  }

  public isParentQuestionAnswered(id: number, parentTriggerValue: string): boolean {
    if (!id) {
      return true;
    }
    if (!isNullOrUndefined(parentTriggerValue)) {
      return +this.questionFormGroup.get('questionsGroup').value['control_' + id] === +parentTriggerValue;
    }
    return this.questionFormGroup.get('questionsGroup').value['control_' + id];
  }

  public getQuestionSelectItems(partitions: ScoringApplicationPartition[]): { [id: number]: SelectItem[] } {
    const selectItems = {};
    for (const partition of partitions) {
      for (const question of partition.questions) {
        if (question.extendedInfo) {
          const selectItemsValues = JSON.parse(question.extendedInfo).Values;
          selectItems[question.id] = selectItemsValues.map(v => {
            return {
              label: this.translateService.instant('EditScoringApplication.' + v),
              value: v
            };
          });
        }
      }
    }
    return selectItems;
  }

  public async onSubmitAsync(): Promise<void> {
    const isValid = this.validateForm();
    if (isValid) {
      await this.saveDraftAsync();
      await this.scoringApplicationApiClient.submitAsync(this.projectId);

      if (this.isPrivateProject) {
        await this.router.navigate([Paths.Project + '/' + this.projectId]);
      } else {
        await this.router.navigate([Paths.Project + '/' + this.projectId + '/payment']);
      }
    }
  }

  public async onSaveAsync(): Promise<void> {
    await this.saveDraftAsync();
    await this.navigateToProjectAsync();
  }

  public getQuestionsWithAnswers(): Answer[] {
    return this.questions.partitions
      .selectMany(p => p.questions)
      .map(q => {
        let questionValue = typeof(this.questionsGroup.value['control_' + q.id]) === 'boolean' ?
          +this.questionsGroup.value['control_' + q.id] : this.questionsGroup.value['control_' + q.id];

        if (q.parentId) {
          const parentValue = this.questionsGroup.value['control_' + q.parentId] + '';
          if (parentValue !== q.parentTriggerValue) {
            questionValue = null;
          }
        }

        return <Answer>{
          questionId: q.id,
          value: questionValue
        };
      });
  }

  public getSocialsValues(): any {
    const socialEnum = SocialMediaTypeEnum;
    const socialsValues = {};

    for (const social of this.activeSocials) {
      socialsValues[socialEnum[this.socialsGroup.value['social_' + social]]] = this.socialsGroup.value['social-link_' + social];
    }
    return socialsValues;
  }

  public getTeamMembers(): TeamMember[] {
    return this.activeTeamMembers.map(t => <TeamMember>{
      fullName: this.teamGroup.value['team_member_name_' + t],
      projectRole: this.teamGroup.value['team_member_role_' + t],
      about: this.teamGroup.value['team_member_experience_' + t],
      facebookLink: this.teamGroup.value['team_member_facebook_' + t],
      linkedInLink: this.teamGroup.value['team_member_linkedin_' + t],
      additionalInformation: this.teamGroup.value['team_experience_name_' + t]
    });
  }

  public getAdvisers(): Adviser[] {
    return this.activeAdvisers.map(a => <Adviser>{
      fullName: this.advisersGroup.value['adviser_name_' + a],
      about: this.advisersGroup.value['adviser_about_' + a],
      reason: this.advisersGroup.value['adviser_reason_' + a],
      facebookLink: this.advisersGroup.value['adviser_facebook_' + a],
      linkedInLink: this.advisersGroup.value['adviser_linkedin_' + a]
    });
  }

  public addSocialMedia(): void {
    this.socialsGroup.addControl('social_' + (this.activeSocials.length + 1), new FormControl('', [Validators.required]));
    this.socialsGroup.addControl('social-link_' + (this.activeSocials.length + 1),
      new FormControl('', [Validators.required, Validators.pattern('https?://.+')]));
    this.activeSocials.push(this.activeSocials.length + 1);
  }

  public setSocialMedia(network?: number, value?: string): void {
    this.socialsGroup.controls['social_' + this.activeSocials[this.activeSocials.length - 1]].setValue(network);
    this.socialsGroup.controls['social-link_' + this.activeSocials[this.activeSocials.length - 1]].setValue(value);
  }

  public removeSocialMedia(id: number): void {
    this.socialsGroup.removeControl('social_' + id);
    this.socialsGroup.removeControl('social-link_' + id);
    this.activeSocials = this.activeSocials.filter(a => a !== id);
  }

  public addArticleLink(): void {
    this.socialsGroup.addControl('article-link_' + (this.activeArticleLink.length + 1),
      new FormControl('', [Validators.required, Validators.pattern('https?://.+')]));
    this.activeArticleLink.push(this.activeArticleLink.length + 1);
  }

  public removeArticleLink(id: number): void {
    this.socialsGroup.removeControl('article-link_' + id);
    this.activeArticleLink = this.activeArticleLink.filter(a => a !== id);
  }

  public addTeamMember(): void {
    const newTeamMemberNumber = this.activeTeamMembers.length === 0 ? 1 : this.activeTeamMembers[this.activeTeamMembers.length - 1] + 1;
    this.teamGroup.addControl('team_member_name_' + newTeamMemberNumber, new FormControl('', [Validators.required]));
    this.teamGroup.addControl('team_member_role_' + newTeamMemberNumber, new FormControl('', [Validators.required]));
    this.teamGroup.addControl('team_member_linkedin_' + newTeamMemberNumber, new FormControl('', [Validators.pattern('https?://.+')]));
    this.teamGroup.addControl('team_member_facebook_' + newTeamMemberNumber, new FormControl('', [Validators.pattern('https?://.+')]));
    this.teamGroup.addControl('team_member_experience_' + newTeamMemberNumber, new FormControl('', [Validators.required]));
    this.activeTeamMembers.push(newTeamMemberNumber);
  }

  public setTeamMember(id?: number, name?: string, role?: string, linkedin?: string, facebook?: string, description?: string): void {
    this.teamGroup.controls['team_member_name_' + this.activeTeamMembers[this.activeTeamMembers.length - 1]].setValue(name);
    this.teamGroup.controls['team_member_role_' + this.activeTeamMembers[this.activeTeamMembers.length - 1]].setValue(role);
    this.teamGroup.controls['team_member_linkedin_' + this.activeTeamMembers[this.activeTeamMembers.length - 1]].setValue(linkedin);
    this.teamGroup.controls['team_member_facebook_' + this.activeTeamMembers[this.activeTeamMembers.length - 1]].setValue(facebook);
    this.teamGroup.controls['team_member_experience_' + this.activeTeamMembers[this.activeTeamMembers.length - 1]].setValue(description);
  }

  public removeTeamMember(id): void {
    this.teamGroup.removeControl('team_member_name_' + id);
    this.teamGroup.removeControl('team_member_role_' + id);
    this.teamGroup.removeControl('team_member_linkedin_' + id);
    this.teamGroup.removeControl('team_member_facebook_' + id);
    this.teamGroup.removeControl('team_member_experience_' + id);
    this.activeTeamMembers = this.activeTeamMembers.filter(a => a !== id);
  }

  public addAdviser(): void {
    const newAdviserNumber = this.activeAdvisers.length === 0 ? 1 : this.activeAdvisers[this.activeAdvisers.length - 1] + 1;
    this.advisersGroup.addControl('adviser_name_' + newAdviserNumber, new FormControl('', [Validators.required]));
    this.advisersGroup.addControl('adviser_about_' + newAdviserNumber, new FormControl('', [Validators.required]));
    this.advisersGroup.addControl('adviser_reason_' + newAdviserNumber, new FormControl('', [Validators.required]));
    this.advisersGroup.addControl('adviser_facebook_' + newAdviserNumber, new FormControl('', [Validators.pattern('https?://.+')]));
    this.advisersGroup.addControl('adviser_linkedin_' + newAdviserNumber, new FormControl('', [Validators.pattern('https?://.+')]));
    this.activeAdvisers.push(newAdviserNumber);
  }

  public setAdviser(reason?: string, fullName?: string, about?: string, facebookLink?: string, linkedInLink?: string): void {
    this.advisersGroup.controls['adviser_about_' + this.activeAdvisers[this.activeAdvisers.length - 1]].setValue(about);
    this.advisersGroup.controls['adviser_facebook_' + this.activeAdvisers[this.activeAdvisers.length - 1]].setValue(facebookLink);
    this.advisersGroup.controls['adviser_linkedin_' + this.activeAdvisers[this.activeAdvisers.length - 1]].setValue(linkedInLink);
    this.advisersGroup.controls['adviser_name_' + this.activeAdvisers[this.activeAdvisers.length - 1]].setValue(fullName);
    this.advisersGroup.controls['adviser_reason_' + this.activeAdvisers[this.activeAdvisers.length - 1]].setValue(reason);
  }

  public removeAdviser(id): void {
    this.advisersGroup.removeControl('adviser_name_' + id);
    this.advisersGroup.removeControl('adviser_about_' + id);
    this.advisersGroup.removeControl('adviser_reason_' + id);
    this.advisersGroup.removeControl('adviser_facebook_' + id);
    this.advisersGroup.removeControl('adviser_linkedin_' + id);
    this.activeAdvisers = this.activeAdvisers.filter(a => a !== id);
  }

  private validateForm(): boolean {
    const invalidElements = this.requiredFields.filter(
      (i) => {
        let elem = false;

        if (i.el) {
          elem = i.el.nativeElement.classList.contains('ng-invalid');
        }

        if (i.elementRef) {
          elem = i.elementRef.nativeElement.classList.contains('ng-invalid');
        }

        if (i.nativeElement) {
          if (i.nativeElement.nativeElement) {
            return i.nativeElement.nativeElement.classList.contains('ng-invalid');
          }
          elem = i.nativeElement.classList.contains('ng-invalid');
        }
        return elem;
      });

    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        const element = invalidElements[a].el !== undefined ? invalidElements[a].el : invalidElements[a];
        this.setInvalid(element);
      }
      const firstElement = invalidElements[0].el !== undefined ? invalidElements[0].el : invalidElements[0];
      this.scrollToElement(firstElement);
      return false;
    }

    if (this.activeSocials.length === 0 && this.activeArticleLink.length === 0) {
      this.scrollToElement(this.socialsContainer);
      this.notificationsService.error(
        this.translateService.instant('Common.RequiredSocialsErrorTitle'),
        this.translateService.instant('Common.RequiredSocialsErrorMessage')
      );
      return false;
    }

    if (this.activeTeamMembers.length === 0) {
      this.scrollToElement(this.membersContainer);
      this.notificationsService.error(
        this.translateService.instant('Common.RequiredMembersErrorTitle'),
        this.translateService.instant('Common.RequiredMembersErrorMessage')
      );
      return false;
    }

    return true;
  }

  private scrollToElement(element): void {
    if (element.elementRef) {
      const offsetTop1 = element.elementRef.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
      return;
    }
    if (element.nativeElement.nativeElement) {
      const offsetTop1 = element.nativeElement.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
      return;
    }
    if (element.nativeElement) {
      const offsetTop1 = element.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
    }
  }

  private setInvalid(element): void {
    if (element.elementRef) {
      element.elementRef.nativeElement.classList.add('ng-invalid');
      element.elementRef.nativeElement.classList.add('ng-dirty');
      return;
    }
    if (element.nativeElement.nativeElement) {
      element.nativeElement.nativeElement.classList.add('ng-invalid');
      element.nativeElement.nativeElement.classList.add('ng-dirty');
      return;
    }
    if (element.nativeElement) {
      element.nativeElement.classList.add('ng-invalid');
      element.nativeElement.classList.add('ng-dirty');
    }
  }

  private async saveDraftAsync(): Promise<void> {
    this.disabled = true;
    const socials = this.getSocialsValues();
    const draftRequest = <SaveScoringApplicationRequest>{
      projectName: this.questionFormGroup.get('commonGroup').get('name').value,
      projectCategory: this.questionFormGroup.get('commonGroup').get('projectCategory').value,
      projectStage: this.questionFormGroup.get('commonGroup').get('projectStage').value,
      projectDescription: this.questionFormGroup.get('commonGroup').get('description').value,
      countryCode: this.questionFormGroup.get('commonGroup').get('country').value,
      site: this.questionFormGroup.get('commonGroup').get('website').value,
      whitePaper: this.questionFormGroup.get('commonGroup').get('linkToWP').value,
      icoDate: this.questionFormGroup.get('commonGroup').get('icoDate').value,
      contactEmail: this.questionFormGroup.get('commonGroup').get('email').value,
      socialNetworks: socials,
      linkedIn: socials['LinkedIn'],
      answers: this.getQuestionsWithAnswers(),
      teamMembers: this.getTeamMembers(),
      advisers: this.getAdvisers(),
      articles: this.getArticles()
    };
    await this.scoringApplicationApiClient.saveAsync(this.projectId, draftRequest);
    this.disabled = false;
    this.savedTime = new Date(Date.now());
  }

  private async navigateToProjectAsync(): Promise<void> {
    await this.router.navigate([Paths.Project + '/' + this.projectId + '/details/application']);
  }

  private getArticles(): string {
    const articleLinks = [];
    for (const link of this.activeArticleLink) {
      articleLinks.push(this.socialsGroup.value['article-link_' + link]);
    }
    return JSON.stringify(articleLinks);
  }

  private loadEditedQuestion(partitions): void {
    partitions.selectMany(partition => partition.questions)
      .map(question => this.questionsGroup.controls['control_' + question.id].setValue(question.answer));
  }

  public onAppear(event, id) {
    if (event.status) {
      this.activePartition = id;
    }
  }

  public async showAlertModal() {
      const submit = await this.dialogService.showPrivateScoringApplicationDialog();
      if (submit) {
          await this.onSubmitAsync();
      }
  }
}
