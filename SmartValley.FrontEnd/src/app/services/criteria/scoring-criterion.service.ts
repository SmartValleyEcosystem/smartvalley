import {Injectable} from '@angular/core';
import {AreaType} from '../../api/scoring/area-type.enum';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {TranslateService} from '@ngx-translate/core';
import {ScoringCriterion} from '../scoring-criterion';
import {ScoringCriterionResponse} from '../../api/estimates/scoring-criterion-response';

@Injectable()
export class ScoringCriterionService {
  private criteria: { [areaType: number]: Array<ScoringCriterion>; } = {};

  constructor(private estimatesClient: EstimatesApiClient,
              private translateService: TranslateService) {
  }

  public getByArea(area: AreaType): Array<ScoringCriterion> {
    return this.criteria[area];
  }

  public async initializeAsync(): Promise<void> {
    const responses = await this.estimatesClient.getScoringCriteriaAsync();
    const criteria = await Promise.all(responses.items.map(r => this.createScoringCriterion(r)));
    for (const item in AreaType) {
      if (Number(item)) {
        this.criteria[item] = criteria.filter(i => i.area === parseInt(item, 0));
      }
    }
  }

  private async createScoringCriterion(response: ScoringCriterionResponse): Promise<ScoringCriterion> {
    return <ScoringCriterion> {
      id: response.id,
      area: response.areaType,
      description: await this.translateService.get('CriteriaDescriptions.' + response.id).toPromise()
    };
  }
}
