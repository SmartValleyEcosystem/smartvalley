import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {EstimateResponse} from './estimate-response';
import {ScoringCategory} from '../scoring/scoring-category.enum';

@Injectable()
export class EstimatesApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async submitEstimatesAsync(request: SubmitEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates', request).toPromise();
  }

  async getByProjectIdAndCategoryAsync(projectId: number, category: ScoringCategory): Promise<CollectionResponse<EstimateResponse>> {
    return this.http
      .get<CollectionResponse<EstimateResponse>>(this.baseApiUrl + '/estimates?projectId=' + projectId + '&category=' + category)
      .toPromise();
  }
}
