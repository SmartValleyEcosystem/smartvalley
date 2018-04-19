import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {AreaType} from '../scoring/area-type.enum';
import {GetEstimatesResponse} from './get-estimates-response';
import {AreaScoringCriteriaResponse} from './area-scoring-criteria-response';
import {ScoringAreaConslusionResponse} from '../scoring/scoring-area-conclusion-response';
import {CriterionWithEstimatesResponse} from './criterion-with-estimates-response';
import {ScoringOfferResponse} from '../scoring/scoring-offer-response';
import {EstimateResponse} from './estimate-response';
import {OfferStatus} from '../scoring-offer/offer-status.enum';
import {SaveEstimatesRequest} from './save-estimates-request';

@Injectable()
export class EstimatesApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async submitEstimatesAsync(request: SubmitEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates/submit', request).toPromise();
  }

  async saveEstimatesAsync(request: SaveEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates/', request).toPromise();
  }

  async getAsync(projectId: number): Promise<CollectionResponse<GetEstimatesResponse>> {

    const parameters = new HttpParams()
      .append('projectId', projectId.toString());

    return this.http
      .get<CollectionResponse<GetEstimatesResponse>>(this.baseApiUrl + '/estimates', {params: parameters})
      .toPromise();
  }

  async getScoringCriteriaAsync(): Promise<CollectionResponse<AreaScoringCriteriaResponse>> {
    return await this.http.get<CollectionResponse<AreaScoringCriteriaResponse>>(this.baseApiUrl + '/estimates/criteria').toPromise();
  }
}
