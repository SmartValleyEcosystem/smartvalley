import {HttpClient, HttpParams} from '@angular/common/http';
import {BaseApiClient} from '../base-api-client';
import {Injectable} from '@angular/core';
import {AllotmentEvent} from './allotment-event';
import {AllotmentEventStatus} from './allotment-event-status';
import {ExpertResponse} from '../expert/expert-response';
import {AreaResponse} from '../expert/area-response';
import {CollectionResponse} from '../collection-response';
import {UserResponse} from '../user/user-response';
import {GetAllotmentEventsRequest} from './get-allotment-events-request';

@Injectable()
export class AllotmentEventsApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getAllotmentEvents(params: GetAllotmentEventsRequest): Promise<CollectionResponse<AllotmentEvent>> {
    let statuses = '';

    if (params.status) {
      let statusRequest = '?';
      for (let i = 0; i < params.status.length; i++) {
        if (params.status[i]) {
          statusRequest += 'allotmentEventStatuses=' + i + '&';
        }
      }
      statuses = statusRequest;
    }

    const parameters = new HttpParams()
        .append('offset', params.offset.toString())
        .append('count', params.count.toString());
    return this.http.get<CollectionResponse<AllotmentEvent>>(`${this.baseApiUrl}/allotmentEvents/` + statuses, {
      params: parameters
    }).toPromise();
  }
}
