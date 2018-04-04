import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {ScoringApplicationApiClient} from '../../../api/scoring-application/scoring-application-api-client';
import {isNullOrUndefined} from 'util';
import {ProjectApplicationInfoResponse} from '../../../api/scoring-application/project-application-info-response';
import {ScoringApplicationPartition} from '../../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../../api/scoring-application/question-control-type.enum';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {UserContext} from '../../../services/authentication/user-context';

@Component({
  selector: 'app-scoring-application',
  templateUrl: './scoring-application.component.html',
  styleUrls: ['./scoring-application.component.css']
})
export class ScoringApplicationComponent implements OnInit {

  @Input() public projectId: number;
  public projectInfo: ProjectApplicationInfoResponse;
  public haveSocials: boolean;
  public partitions: ScoringApplicationPartition[];
  public questionControlType = QuestionControlType;
  public questionTypeLine = QuestionControlType[0];
  public questionTypeMultiLine = QuestionControlType[1];
  public questionTypeCombobox = QuestionControlType[2];
  public questionTypeDateTime = QuestionControlType[3];
  public questionTypeCheckBox = QuestionControlType[4];
  public questionTypeUrl = QuestionControlType[5];

  public doesScoringApplicationExists: boolean;
  public isCreateScoringApplicationCommandAvailable = false;
  public isEditScoringApplicationCommandAvailable = false;

  constructor(private router: Router,
              private htmlElement: ElementRef,
              private scoringApplicationApiClient: ScoringApplicationApiClient,
              private projectApiClient: ProjectApiClient,
              private userContext: UserContext) {
  }

  async ngOnInit() {
    const response = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.projectId);

    if (!isNullOrUndefined(response.created)) {
      this.projectInfo = response.projectInfo;
      this.haveSocials = this.checkSocials();
      this.partitions = response.partitions;
      this.doesScoringApplicationExists = true;
    }

    const project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    const currentUser = await this.userContext.getCurrentUser();

    if (!isNullOrUndefined(currentUser) && currentUser.id === project.authorId) {
      this.isCreateScoringApplicationCommandAvailable = isNullOrUndefined(response.created);
      this.isEditScoringApplicationCommandAvailable = !isNullOrUndefined(response.created);
    }
  }

  private checkSocials(): boolean {
    return !isNullOrUndefined(this.projectInfo.socialNetworks.bitcoinTalk) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.facebook) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.github) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.linkedin) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.medium) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.reddit) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.telegram) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.twitter)
      ;
  }

  public getQuestionTypeById(id: number) {
    return this.questionControlType[id];
  }

  public isParentQuestionAnswered(id: number) {
    if (!id) {
      return true;
    }
    return this.partitions.map(i => i.questions).reduce((l, r) => l.concat(r)).some(i => i.id === id);
  }

  public scrollToElement(elementId: string) {
    const element = this.htmlElement.nativeElement.querySelector('#partition_' + elementId);
    const containerOffset = element.offsetTop;
    const fieldOffset = element.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset + 500, behavior: 'smooth'});
  }

  public navigateToCreateForm() {
    this.router.navigate([Paths.ScoringApplication + '/' + this.projectId]);
  }

  public navigateToEdit() {
    this.router.navigate([Paths.ScoringApplication + '/' + this.projectId]);
  }
}
