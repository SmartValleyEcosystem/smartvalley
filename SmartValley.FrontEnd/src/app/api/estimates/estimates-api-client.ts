import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {EstimateResponse} from './estimate-response';
import {GetEstimatesRequest} from './get-estimates-request';

@Injectable()
export class EstimatesApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async submitEstimatesAsync(request: SubmitEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates', request).toPromise();
  }

  async getByProjectIdAndCategoryAsync(request: GetEstimatesRequest): Promise<CollectionResponse<EstimateResponse>> {
    return this.http
      .get<CollectionResponse<EstimateResponse>>(this.baseApiUrl + '/estimates?projectId=' + request.projectId + '&category=' + request.category)
      .toPromise();
  }
}
