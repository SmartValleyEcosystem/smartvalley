import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {AreaType} from '../scoring/area-type.enum';
import {GetEstimatesResponse} from './get-estimates-response';
import {ScoringCriterionResponse} from './scoring-criterion-response';

@Injectable()
export class EstimatesApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async submitEstimatesAsync(request: SubmitEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates', request).toPromise();
  }

  async getAsync(projectId: number, areaType: AreaType): Promise<GetEstimatesResponse> {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('areaType', areaType.toString());

    return this.http
      .get<GetEstimatesResponse>(this.baseApiUrl + '/estimates', {params: parameters})
      .toPromise();
  }

  async getScoringCriteriaAsync(): Promise<CollectionResponse<ScoringCriterionResponse>> {
    return await this.http.get<CollectionResponse<ScoringCriterionResponse>>(this.baseApiUrl + '/estimates/criteria').toPromise();
  }
}
