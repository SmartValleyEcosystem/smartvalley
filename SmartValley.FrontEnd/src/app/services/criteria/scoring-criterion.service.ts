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
    const scoringCtiteriaGroup = {
      name: null,
      order: null,
      criteria: null
    };
    this.translateService.get(`CriteriaGroups.${response.key}`).toPromise().then((result) => {
      scoringCtiteriaGroup['name'] = result;
    } );
    scoringCtiteriaGroup['order'] = response.order;
    scoringCtiteriaGroup['criteria'] = response.criteria.map(c => this.createCriterion(c));

    return scoringCtiteriaGroup;
  }

  private createCriterion(response: ScoringCriterionResponse): ScoringCriterion {
    const scoringCriterion = {
      id: null,
      name: null,
      weight: null,
      order: null,
      options: null
    };
    scoringCriterion.id =  response.id;
    this.translateService.get(`CriteriaNames.${response.id}`).toPromise().then((result) => {
      scoringCriterion.name = result;
    } );
    scoringCriterion.order = response.order;
    scoringCriterion.weight = response.weight;
    scoringCriterion.options = this.getCriterionOptions(response.id, response.hasMiddleScoreOption);

    return <ScoringCriterion>scoringCriterion;
  }

  private getCriterionOptions(scoringCriterionId: number, hasMiddleScoreOption: boolean): Array<ScoringCriterionOption> {
    const scores = hasMiddleScoreOption ? [Score.Low, Score.Medium, Score.High] : [Score.Low, Score.High];

    const scoringCriterionOptions = [];
    for (const score of scores) {
     this.translateService
        .get(`CriteriaOptionDescriptions.${scoringCriterionId}.${<number>score}`)
        .toPromise()
        .then(result => scoringCriterionOptions.push({
          score: score,
          description: result
        }));
    }

    return scoringCriterionOptions;
  }
}
