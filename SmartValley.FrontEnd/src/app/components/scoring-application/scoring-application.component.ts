import {Component, ElementRef, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ScoringApplicationResponse} from '../../api/scoring-application/scoring-application-response';
import {ScoringApplicationPartition} from '../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../api/scoring-application/question-control-type.enum';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../../services/project/social-media-type.enum';
import {TeamMemberItem} from '../../api/scoring-application/team-member-item';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {Answer} from '../../api/scoring-application/answer';

@Component({
  selector: 'app-scoring-application',
  templateUrl: './scoring-application.component.html',
  styleUrls: ['./scoring-application.component.css']
})
export class ScoringApplicationComponent implements OnInit {

  public projectId: number;
  public questions: ScoringApplicationResponse;
  public partitions: ScoringApplicationPartition[];
  public questionControlType = QuestionControlType;
  public questionTypeLine = QuestionControlType[0];
  public questionTypeMultiLine = QuestionControlType[1];
  public questionTypeCombobox = QuestionControlType[2];
  public questionTypeDateTime = QuestionControlType[3];
  public questionTypeCheckBox = QuestionControlType[4];
  public questionTypeUrl = QuestionControlType[5];
  public questionFormGroup: FormGroup;
  public activeSocials: number[] = [];
  public activeArticleLink: number[] = [];
  public activeTeamMembers: number[] = [];
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
    this.categories =  this.dictionariesService.categories.map(i => <SelectItem>{
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

  public createCommonForm() {
    const commonFormControls = {
      name: [''],
      country: [''],
      stage: [''],
      projectArea: [''],
      icoDate: [''],
      raised: [''],
      website: [''],
      description: [''],
      linkToWP: [''],
      email: ['']
    };
    this.questionFormGroup = this.formBuilder.group({
      commonGroup: this.formBuilder.group(commonFormControls),
      socialsGroup: this.formBuilder.group([]),
      questionsGroup: this.formBuilder.group([]),
      teamGroup: this.formBuilder.group([])
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
    this.saveDraftAsync();
    await this.scoringApplicationApiClient.submitScoringApplicationProjectAsync(this.projectId);
  }

  public async saveDraftAsync() {
    const socials = this.getSocialsValues();
    const draftRequest = {
      projectName: this.questionFormGroup.get('commonGroup').get('name').value,
      projectArea: this.questionFormGroup.get('commonGroup').get('projectArea').value,
      status: this.questionFormGroup.get('commonGroup').get('stage').value,
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
      answers: this.getAnswers(),
      teamMembers: this.getTeamMembers(),
      advisers: []
    };
    await this.scoringApplicationApiClient.saveScoringApplicationProjectAsync(this.projectId, draftRequest);
  }

  public getAnswers(): Answer[] {
    const answers = [];
    for ( const partition of this.questions.partitions ) {
      for (const question of partition.questions) {
        const answer = {
          questionId: question.id,
          value: this.questionsGroup.value['control_' + question.id]
        };
        answers.push(answer);
      }
    }
    return answers;
  }

  public getSocialsValues() {

    const socialEnum = SocialMediaTypeEnum;
    let socialsValues = {};

    for (let social of this.activeSocials) {
      socialsValues[socialEnum[this.socialsGroup.value['social_' + social]]] = this.socialsGroup.value['social-link_' + social];
    }

    return socialsValues;
  }

  public getTeamMembers(): TeamMemberItem[] {
    let teamMembers: TeamMemberItem[] = [];
    for (let member of this.activeTeamMembers) {
      let currentMember = {
        fullName: this.teamGroup.value['team_member_name_' + member],
        projectRole: this.teamGroup.value['team_member_role_' + member],
        about: this.teamGroup.value['team_member_experience_' + member],
        facebookLink: this.teamGroup.value['team_member_facebook_' + member],
        linkedInLink: this.teamGroup.value['team_member_linkedin_' + member],
        additionalInformation: this.teamGroup.value['team_experience_name_' + member]
      };
      teamMembers.push(currentMember);
    }
    return teamMembers;
  }

  public addSocialMedia() {
    this.socialsGroup.addControl('social_' + (this.activeSocials.length + 1), new FormControl(''));
    this.socialsGroup.addControl('social-link_' + (this.activeSocials.length + 1), new FormControl(''));
    this.activeSocials.push(this.activeSocials.length + 1);
  }

  public removeSocialMedia(id: number) {
    this.activeSocials = this.activeSocials.filter( a => a !== id);
  }

  public addArticleLink() {
    this.socialsGroup.addControl('article-link_' + (this.activeArticleLink.length + 1), new FormControl(''));
    this.activeArticleLink.push(this.activeArticleLink.length + 1);
  }

  public removeArticleLink(id: number) {
    this.activeArticleLink = this.activeArticleLink.filter( a => a !== id);
  }

  public addTeamMember() {
    this.teamGroup.addControl('team_member_name_' + (this.activeTeamMembers.length + 1), new FormControl(''));
    this.teamGroup.addControl('team_member_role_' + (this.activeTeamMembers.length + 1), new FormControl(''));
    this.teamGroup.addControl('team_member_linkedin_' + (this.activeTeamMembers.length + 1), new FormControl(''));
    this.teamGroup.addControl('team_member_facebook_' + (this.activeTeamMembers.length + 1), new FormControl(''));
    this.teamGroup.addControl('team_member_experience_' + (this.activeTeamMembers.length + 1), new FormControl(''));
    this.activeTeamMembers.push(this.activeTeamMembers.length + 1);
  }

  public removeTeamMember(id) {
    this.activeTeamMembers = this.activeTeamMembers.filter( a => a !== id);
  }
}
