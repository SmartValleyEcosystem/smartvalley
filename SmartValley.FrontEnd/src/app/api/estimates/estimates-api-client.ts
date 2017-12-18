import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitEstimatesRequest} from './submit-estimates-request';
import {CollectionResponse} from '../collection-response';
import {ExpertiseArea} from '../scoring/expertise-area.enum';
import {GetEstimatesResponse} from './get-estimates-response';
import {QuestionResponse} from './question-response';

@Injectable()
export class EstimatesApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async submitEstimatesAsync(request: SubmitEstimatesRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/estimates', request).toPromise();
  }

  async getAsync(projectId: number, expertiseArea: ExpertiseArea): Promise<GetEstimatesResponse> {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('expertiseArea', expertiseArea.toString());

    return this.http
      .get<GetEstimatesResponse>(this.baseApiUrl + '/estimates', {params: parameters})
      .toPromise();
  }

  async getQuestionsAsync(): Promise<CollectionResponse<QuestionResponse>> {
    return await this.http.get<CollectionResponse<QuestionResponse>>(this.baseApiUrl + '/estimates/questions').toPromise();
  }
}
