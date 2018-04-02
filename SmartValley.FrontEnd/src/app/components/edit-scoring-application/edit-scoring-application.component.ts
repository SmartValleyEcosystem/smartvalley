import {Component, ElementRef, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ScoringApplicationResponse} from '../../api/scoring-application/scoring-application-response';
import {ScoringApplicationPartition} from '../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../api/scoring-application/question-control-type.enum';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../../services/project/social-media-type.enum';
import {TeamMember} from '../../api/scoring-application/team-member';
import {SaveScoringApplicationRequest} from '../../api/scoring-application/save-scoring-application-request';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {Answer} from '../../api/scoring-application/answer';
import {Adviser} from '../../api/scoring-application/adviser';

@Component({
  selector: 'app-edit-scoring-application',
  templateUrl: './edit-scoring-application.component.html',
  styleUrls: ['./edit-scoring-application.component.css']
})
export class EditScoringApplicationComponent implements OnInit {

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

  constructor(private scoringApplicationApiClient: ScoringApplicationApiClient,
              private translateService: TranslateService,
              private dictionariesService: DictionariesService,
              private formBuilder: FormBuilder,
              private htmlElement: ElementRef,
              private route: ActivatedRoute) {
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
    this.addQuestionsFormControls(this.partitions);
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

  public createCommonForm() {
    const commonFormControls = {
      name: [''],
      country: [''],
      projectStage: [''],
      projectCategory: [''],
      icoDate: [''],
      website: [''],
      description: [''],
      linkToWP: [''],
      email: ['']
    };
    this.questionFormGroup = this.formBuilder.group({
      commonGroup: this.formBuilder.group(commonFormControls),
      socialsGroup: this.formBuilder.group([]),
      questionsGroup: this.formBuilder.group([]),
      teamGroup: this.formBuilder.group([]),
      advisersGroup: this.formBuilder.group([])
    });
  }

  public scrollToElement(elementId: string) {
    const element = this.htmlElement.nativeElement.querySelector('#partition_' + elementId);
    const containerOffset = element.offsetTop;
    const fieldOffset = element.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
  }

  public addQuestionsFormControls(partitions) {
    for (const partition of partitions) {
      for (const question of partition.questions) {
        this.questionsGroup.addControl('control_' + question.id, new FormControl(''));
      }
    }
  }

  public getQuestionTypeById(id: number) {
    return this.questionControlType[id];
  }

  public isParentQuestionAnswered(id: number) {
    if (!id) {
      return true;
    }
    return this.questionFormGroup.get('questionsGroup').value['control_' + id];
  }

  public async onSubmit() {
    await this.saveDraftAsync();
    await this.scoringApplicationApiClient.submitScoringApplicationProjectAsync(this.projectId);
  }

  public async saveDraftAsync() {
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
      facebookLink: socials['Facebook'],
      bitcointalkLink: socials['BitcoinTalk'],
      mediumLink: socials['Medium'],
      redditLink: socials['Reddit'],
      telegramLink: socials['Telegram'],
      twitterLink: socials['Twitter'],
      gitHubLink: socials['Github'],
      linkedInLink: socials['LinkedIn'],
      answers: this.getQuestionsWithAnswers(),
      teamMembers: this.getTeamMembers(),
      advisers: this.getAdvisers()
    };
    await this.scoringApplicationApiClient.saveScoringApplicationProjectAsync(this.projectId, draftRequest);
  }

  public getQuestionsWithAnswers(): Answer[] {
    return this.questions.partitions
      .map(p => p.questions)
      .reduce((a, b) => a.concat(b))
      .map(q => <Answer>{
        questionId: q.id,
        value: this.questionsGroup.value['control_' + q.id]
      });
  }

  public getSocialsValues() {
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

  public addSocialMedia() {
    this.socialsGroup.addControl('social_' + (this.activeSocials.length + 1), new FormControl(''));
    this.socialsGroup.addControl('social-link_' + (this.activeSocials.length + 1), new FormControl(''));
    this.activeSocials.push(this.activeSocials.length + 1);
  }

  public removeSocialMedia(id: number) {
    this.activeSocials = this.activeSocials.filter(a => a !== id);
  }

  public addArticleLink() {
    this.socialsGroup.addControl('article-link_' + (this.activeArticleLink.length + 1), new FormControl(''));
    this.activeArticleLink.push(this.activeArticleLink.length + 1);
  }

  public removeArticleLink(id: number) {
    this.activeArticleLink = this.activeArticleLink.filter(a => a !== id);
  }

  public addTeamMember() {
    const newTeamMemberNumber = (this.activeTeamMembers.length + 1);
    this.teamGroup.addControl('team_member_name_' + newTeamMemberNumber, new FormControl(''));
    this.teamGroup.addControl('team_member_role_' + newTeamMemberNumber, new FormControl(''));
    this.teamGroup.addControl('team_member_linkedin_' + newTeamMemberNumber, new FormControl(''));
    this.teamGroup.addControl('team_member_facebook_' + newTeamMemberNumber, new FormControl(''));
    this.teamGroup.addControl('team_member_experience_' + newTeamMemberNumber, new FormControl(''));
    this.activeTeamMembers.push(newTeamMemberNumber);
  }

  public addAdviser() {
    const newAdviserNumber = this.activeAdvisers.length + 1;
    this.advisersGroup.addControl('adviser_name_' + newAdviserNumber, new FormControl(''));
    this.advisersGroup.addControl('adviser_about_' + newAdviserNumber, new FormControl(''));
    this.advisersGroup.addControl('adviser_reason_' + newAdviserNumber, new FormControl(''));
    this.advisersGroup.addControl('adviser_facebook_' + newAdviserNumber, new FormControl(''));
    this.advisersGroup.addControl('adviser_linkedin_' + newAdviserNumber, new FormControl(''));
    this.activeAdvisers.push(newAdviserNumber);
  }

  public removeTeamMember(id) {
    this.activeTeamMembers = this.activeTeamMembers.filter(a => a !== id);
  }
}
