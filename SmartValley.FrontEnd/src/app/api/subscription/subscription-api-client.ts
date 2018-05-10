import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {SubscriptionResponse} from './subscription-response';
import {SubscribeRequest} from '../../components/common/subscribe-modal/subscribe-data';

@Injectable()
export class SubscriptionApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async subscribeAsync(request: SubscribeRequest, projectId: number): Promise<void> {
    await this.http.post(this.baseApiUrl + `/subscriptions/${projectId}`, request).toPromise();
  }

  public getAsync(offset: number, count: number): Promise<CollectionResponse<SubscriptionResponse>> {
    const parameters = new HttpParams()
      .append('offset', offset.toString())
      .append('count', count.toString());

    return this.http.get<CollectionResponse<SubscriptionResponse>>(`${this.baseApiUrl}/subscriptions`, {
      params: parameters
    }).toPromise();
  }
}
