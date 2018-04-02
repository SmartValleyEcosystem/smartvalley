import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {ScoringApplicationApiClient} from '../../../api/scoring-application/scoring-application-api-client';
import {isNullOrUndefined} from 'util';
import {ProjectApplicationInfoResponse} from '../../../api/scoring-application/project-application-info-response';
import {ScoringApplicationPartition} from '../../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../../api/scoring-application/question-control-type.enum';

@Component({
  selector: 'app-scoring-application',
  templateUrl: './scoring-application.component.html',
  styleUrls: ['./scoring-application.component.css']
})
export class ScoringApplicationComponent implements OnInit {

  @Input() public projectId: number;
  public isExistApplication: boolean;
  public projectInfo: ProjectApplicationInfoResponse;
  public partitions: ScoringApplicationPartition[];
  public questionControlType = QuestionControlType;
  public questionTypeLine = QuestionControlType[0];
  public questionTypeMultiLine = QuestionControlType[1];
  public questionTypeCombobox = QuestionControlType[2];
  public questionTypeDateTime = QuestionControlType[3];
  public questionTypeCheckBox = QuestionControlType[4];
  public questionTypeUrl = QuestionControlType[5];

  constructor(private router: Router,
              private htmlElement: ElementRef,
              private scoringApplicationApiClient: ScoringApplicationApiClient) {
  }

  async ngOnInit() {
    const response = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.projectId);
    if (!isNullOrUndefined(response.created)) {
      this.projectInfo = response.projectInfo;
      this.partitions = response.partitions;
      this.isExistApplication = true;
    }
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
    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
  }

  public navigateToCreateForm() {
    this.router.navigate([Paths.ScoringApplication + this.projectId]);
  }
}
