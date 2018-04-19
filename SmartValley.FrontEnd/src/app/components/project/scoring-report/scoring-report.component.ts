import {Component, Input, OnInit} from '@angular/core';
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

@Component({
  selector: 'app-scoring-report',
  templateUrl: './scoring-report.component.html',
  styleUrls: ['./scoring-report.component.scss']
})
export class ScoringReportComponent implements OnInit {

  public areasScoringInfo: AreasScoringInfo[] = [];
  public scoringCriterionResponse: ScoringCriteriaGroup[] = [];

  @Input() projectId: number;
  @Input() scoringStatus: ScoringStatus;
  @Input() isAuthor: boolean;

  constructor(private router: Router,
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
    }
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
}
