import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {FeedbackRequest} from '../../components/common/feedback-modal/feedback';
import {CollectionResponse} from '../collection-response';
import {FeedbackResponse} from './feedback-response';

@Injectable()
export class FeedbackApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async sendFeedbackAsync(request: FeedbackRequest): Promise<void> {
      await this.http.post(this.baseApiUrl + '/feedbacks', request).toPromise();
  }

  public getFeedbacksListAsync(offset: number, count: number): Promise<CollectionResponse<FeedbackResponse>> {
    const parameters = new HttpParams()
      .append('offset', offset.toString())
      .append('count', count.toString());

    return this.http.get<CollectionResponse<FeedbackResponse>>(`${this.baseApiUrl}/feedbacks`, {
      params: parameters
    }).toPromise();
  }
}
