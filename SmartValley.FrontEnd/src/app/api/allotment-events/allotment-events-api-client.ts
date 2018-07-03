import {HttpClient, HttpParams} from '@angular/common/http';
import {BaseApiClient} from '../base-api-client';
import {Injectable} from '@angular/core';
import {CollectionResponse} from '../collection-response';
import {AllotmentEventResponse} from './responses/allotment-event-response';
import {GetAllotmentEventsRequest} from './request/get-allotment-events-request';
import {CreateAllotmentEventRequest} from './request/create-allotment-event-request';
import {CreateAllotmentEventResponse} from './responses/create-allotment-event-response';
import {TokenBalanceResponse} from './responses/token-balance-response';

@Injectable()
export class AllotmentEventsApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getAllotmentEventsAsync(params: GetAllotmentEventsRequest): Promise<CollectionResponse<AllotmentEventResponse>> {
    let parameters = new HttpParams()
      .append('offset', params.offset.toString())
      .append('count', params.count.toString());

    if (params.statuses) {
      params.statuses.forEach(status => {
        parameters = parameters.append('allotmentEventStatuses', status.toString());
      });
    }

    return this.http.get<CollectionResponse<AllotmentEventResponse>>(
      `${this.baseApiUrl}/allotmentEvents`,
      {params: parameters})
      .toPromise();
  }

  public createAsync(name: string,
                     tokenContractAddress: string,
                     tokenDecimals: number,
                     tokenTicker: string,
                     projectId: number,
                     finishDate?: Date): Promise<CreateAllotmentEventResponse> {
    return this.http.post<CreateAllotmentEventResponse>(`${this.baseApiUrl}/allotmentEvents/`, <CreateAllotmentEventRequest>{
      name: name,
      tokenContractAddress: tokenContractAddress,
      tokenDecimals: tokenDecimals,
      tokenTicker: tokenTicker,
      projectId: projectId,
      finishDate: finishDate
    }).toPromise();
  }

  public async getTokensBalancesAsync(eventsIds: Array<number>): Promise<CollectionResponse<TokenBalanceResponse>> {
    let parameters = new HttpParams();

    eventsIds.forEach(eventId => {
      parameters = parameters.append('eventsIds', eventId.toString());
    });
    return this.http.get<CollectionResponse<TokenBalanceResponse>>(`${this.baseApiUrl}/allotmentEvents/tokensBalances`, {
      params: parameters
    }).toPromise();
  }

  public publishAsync(eventId: number, transactionHash: string) {
    this.http.put(
      `${this.baseApiUrl}/allotmentEvents/${eventId}/publish/`,
      {transactionHash: transactionHash})
      .toPromise();
  }

  public updateAsync(eventId: number, transactionHash: string) {
    this.http.put(
      `${this.baseApiUrl}/allotmentEvents/${eventId}`,
      {transactionHash: transactionHash})
      .toPromise();
  }

  public removeAsync(eventId: number, transactionHash: string) {
    const queryString = '?eventId=' + eventId + '&transactionHash=' + transactionHash;
    this.http.delete(this.baseApiUrl + '/allotmentEvents' + queryString).toPromise();
  }

  public startAsync(eventId: number, transactionHash: string) {
    this.http.put(`${this.baseApiUrl}/allotmentEvents/${eventId}/start/`, {transactionHash: transactionHash}).toPromise();
  }

  public participateAsync(eventId: number, transactionHash: string) {
    this.http.put(`${this.baseApiUrl}/allotmentEvents/${eventId}/participate/`, {transactionHash: transactionHash}).toPromise();
  }

  public receiveTokensAsync(eventId: number, transactionHash: string) {
    this.http.put(`${this.baseApiUrl}/allotmentEvents/${eventId}/receiveTokens/`, {transactionHash: transactionHash}).toPromise();
  }
}
