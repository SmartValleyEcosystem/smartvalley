import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './requests/submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {AreaScoringCriteriaResponse} from './responses/area-scoring-criteria-response';
import {SaveEstimatesRequest} from './requests/save-estimates-request';
import {CriterionPromptResponse} from './responses/criterion-prompt-response';
import {ScoringReportResponse} from './responses/scoring-report-response';

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

  async getAsync(projectId: number): Promise<ScoringReportResponse> {

    const parameters = new HttpParams()
      .append('projectId', projectId.toString());

    return this.http
      .get<ScoringReportResponse>(this.baseApiUrl + '/estimates', {params: parameters})
      .toPromise();
  }

  async getCriterionPromptsAsync(projectId: number, areaType: number): Promise<CollectionResponse<CriterionPromptResponse>> {
    const url = `${this.baseApiUrl}/estimates/project/${projectId}/prompts/${areaType}`;
    return this.http.get<CollectionResponse<CriterionPromptResponse>>(url)
      .toPromise();
  }

  async getEstimatesDraftAsync(projectId: number, areaType: number) {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('areaType', areaType.toString());
    return await this.http.get(this.baseApiUrl + `/estimates/offer`, {params: parameters})
      .toPromise();
  }

  async getScoringCriteriaAsync(): Promise<CollectionResponse<AreaScoringCriteriaResponse>> {
    return this.http.get<CollectionResponse<AreaScoringCriteriaResponse>>(this.baseApiUrl + '/estimates/criteria').toPromise();
  }
}
