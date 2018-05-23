import {Component, Inject, Input, OnInit} from '@angular/core';
import {AreaService} from '../../../services/expert/area.service';
import {EstimatesApiClient} from '../../../api/estimates/estimates-api-client';
import {AreasScoringInfo} from './areas-scoring-Info';
import {ScoringCriteriaGroup} from '../../../services/criteria/scoring-criteria-group';
import {ScoringCriterionService} from '../../../services/criteria/scoring-criterion.service';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {OfferStatus} from '../../../api/scoring-offer/offer-status.enum';
import {AreaType} from "../../../api/scoring/area-type.enum";
import {CriterionPromptResponse} from '../../../api/estimates/criterion-prompt-response';
import {CriterionPrompt} from '../../../api/estimates/criterion-prompt';
import {QuestionControlType} from '../../../api/scoring-application/question-control-type.enum';
import {ProjectComponent} from '../project.component';

@Component({
  selector: 'app-scoring-report',
  templateUrl: './scoring-report.component.html',
  styleUrls: ['./scoring-report.component.scss']
})
export class ScoringReportComponent implements OnInit {

  public areasScoringInfo: AreasScoringInfo[] = [];
  public questionTypeComboBox = QuestionControlType.Combobox;
  public scoringCriterionResponse: ScoringCriteriaGroup[] = [];
  public criterionPrompts: CriterionPromptResponse[] = [];
  public questionsActivity: boolean[] = [];
  public areaType: number;
  public criterionIsReady = false;

  public ScoringStatus = ScoringStatus;

  @Input() projectId: number;
  @Input() scoringStatus: ScoringStatus;
  @Input() isAuthor: boolean;

  constructor(@Inject(ProjectComponent) public parent: ProjectComponent,
              private router: Router,
              private areaService: AreaService,
              private estimatesApiClient: EstimatesApiClient,
              private scoringCriterionService: ScoringCriterionService) {
  }

  public async ngOnInit() {
    const estimates = await this.estimatesApiClient.getAsync(this.projectId);
    for (const item of estimates.items) {
      const estimatesForArea = estimates.items.find(e => e.areaType === item.areaType);
      this.areasScoringInfo.push({
        finishedExperts: estimatesForArea.offers.filter(o => o.status === OfferStatus.Finished).length,
        totalExperts: estimatesForArea.requiredExpertsCount,
        areaName: this.areaService.getNameByType(item.areaType),
        scoringInfo: estimatesForArea,
        areaType: item.areaType
      });
      this.scoringCriterionResponse = this.scoringCriterionResponse.concat(this.scoringCriterionService.getByArea(item.areaType));
      const criterionPromptsResponse = await this.estimatesApiClient.getCriterionPromptsAsync(this.projectId, item.areaType);
      this.criterionPrompts = this.criterionPrompts.concat(criterionPromptsResponse.items);
    }
    this.criterionIsReady = true;
    this.questionsActivity = [true];
  }

  public getCriterionInfo(id): CriterionPrompt[] {
    const criterionPrompts = this.criterionPrompts.find((c) => c.scoringCriterionId === id);
    if (criterionPrompts) {
     return criterionPrompts.prompts;
    }
    return null;
  }

  public getMaxScoreByArea(areaType: AreaType): number {
    return this.areaService.getMaxScore(+areaType);
  }

  public getQuestionById(id: number): string {
    return this.scoringCriterionResponse.selectMany(s => s.criteria).find(c => c.id === id ).name;
  }

  public async navigateToApplicationScoringAsync(): Promise<void> {
    await this.router.navigate(['/' + Paths.ScoringApplication + '/' + this.projectId]);
  }

  public getFinishedExperts(areaType: number): number {
    return this.areasScoringInfo.filter(a => a.areaType === areaType)
      .selectMany(e => e.scoringInfo.offers)
      .filter(o => o.status === OfferStatus.Finished)
      .length;
  }

  public chageActiveQuestion(id: number) {
    const indexOfCurrentSelected = this.questionsActivity.indexOf(true);
    this.questionsActivity = this.questionsActivity.map(() => false);
    if (indexOfCurrentSelected !== id) {
      this.questionsActivity[id] = !this.questionsActivity[id];
    }
  }

  public onTabChange () {
    this.questionsActivity = [];
  }
}
