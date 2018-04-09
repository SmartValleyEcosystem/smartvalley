import {Injectable} from '@angular/core';
import {AreaType} from '../../api/scoring/area-type.enum';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {TranslateService} from '@ngx-translate/core';
import {ScoringCriterionResponse} from '../../api/estimates/scoring-criterion-response';
import {ScoringCriteriaGroup} from './scoring-criteria-group';
import {ScoringCriterion} from './scoring-criterion';
import {ScoringCriterionOption} from './scoring-criterion-option';
import {Score} from '../score.enum';
import {ScoringCriteriaGroupResponse} from '../../api/estimates/scoring-criteria-group-response';

@Injectable()
export class ScoringCriterionService {
  private criteria: { [areaType: number]: Array<ScoringCriteriaGroup>; } = {};

  constructor(private estimatesClient: EstimatesApiClient,
              private translateService: TranslateService) {
  }

  public getByArea(area: AreaType): Array<ScoringCriteriaGroup> {
    return this.criteria[area];
  }

  public async initializeAsync(): Promise<void> {
    const responses = await this.estimatesClient.getScoringCriteriaAsync();
    responses.items.forEach(r => this.criteria[r.area] = r.groups.map(g => this.createCriteriaGroup(g)));
  }

  private createCriteriaGroup(response: ScoringCriteriaGroupResponse): ScoringCriteriaGroup {
    return <ScoringCriteriaGroup> {
      name: this.translateService.instant(`CriteriaGroups.${response.key}`),
      order: response.order,
      criteria: response.criteria.map(c => this.createCriterion(c))
    };
  }

  private createCriterion(response: ScoringCriterionResponse): ScoringCriterion {
    return <ScoringCriterion> {
      id: response.id,
      name: this.translateService.instant(`CriteriaNames.${response.id}`),
      order: response.order,
      weight: response.weight,
      options: this.getCriterionOptions(response.id, response.hasMiddleScoreOption)
    };
  }

  private getCriterionOptions(scoringCriterionId: number, hasMiddleScoreOption: boolean): Array<ScoringCriterionOption> {
    const scores = hasMiddleScoreOption ? [Score.Low, Score.Medium, Score.High] : [Score.Low, Score.High];
    return scores.map(s => <ScoringCriterionOption> {
      score: s,
      description: this.translateService.instant(`CriteriaOptionDescriptions.${scoringCriterionId}.${<number>s}`)
    });
  }
}
