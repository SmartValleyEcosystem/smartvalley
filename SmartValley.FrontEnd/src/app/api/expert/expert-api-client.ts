import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {GetExpertStatusResponse} from './get-expert-status-response';
import {CreateExpertApplicationRequest} from './expert-applitacion-request';
import {CollectionResponse} from '../collection-response';
import {PendingExpertApplicationsResponse} from './pending-expert-applications-response';
import {ExpertApplicationResponse} from './expert-application-response';
import {AreaResponse} from './area-response';
import {AcceptExpertApplicationRequest} from './accept-expert-application-request';
import {RejectApplicationRequest} from './reject-application-request';
import {ExpertResponse} from './expert-response';
import {ExpertRequest} from './expert-request';
import {ExpertDeleteRequest} from './expert-delete-request';
import {ExpertUpdateRequest} from './expert-update-request';
import {ExpertAvailabilityStatusResponse} from './expert-availability-status-response';
import {GetExpertsRequest} from './get-experts-request';
import {isNullOrUndefined} from 'util';

@Injectable()
export class ExpertApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getAreasAsync(): Promise<CollectionResponse<AreaResponse>> {
    return this.http.get<CollectionResponse<AreaResponse>>(`${this.baseApiUrl}/experts/areas`).toPromise();
  }

  public getExpertStatusAsync(address: string): Promise<GetExpertStatusResponse> {
    return this.http.get<GetExpertStatusResponse>(`${this.baseApiUrl}/experts/${address}/status`).toPromise();
  }

  public async createApplicationAsync(request: CreateExpertApplicationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/experts/applications', request.body).toPromise();
  }

  public getPendingApplicationsAsync(): Promise<CollectionResponse<PendingExpertApplicationsResponse>> {
    return this.http.get<CollectionResponse<PendingExpertApplicationsResponse>>(`${this.baseApiUrl}/experts/applications`).toPromise();
  }

  public getApplicationByIdAsync(id: number): Promise<ExpertApplicationResponse> {
    return this.http.get<ExpertApplicationResponse>(`${this.baseApiUrl}/experts/applications/${id}`).toPromise();
  }

  public async acceptExpertApplicationAsync(id: number, areasToAccept: number[], transactionHash: string) {
    await this.http.post(`${this.baseApiUrl}/experts/applications/${id}/accept`, <AcceptExpertApplicationRequest>{
      transactionHash: transactionHash,
      areas: areasToAccept
    }).toPromise();
  }

  public async rejectExpertApplicationAsync(id: number, reason: string, transactionHash: string) {
    await this.http.post(`${this.baseApiUrl}/experts/applications/${id}/reject`, <RejectApplicationRequest>{
      transactionHash: transactionHash,
      reason: reason
    }).toPromise();
  }

  public getExpertsListAsync(getExpertsRequest: GetExpertsRequest): Promise<CollectionResponse<ExpertResponse>> {
    const checkParam = (param) => isNullOrUndefined(param) ? '' : param.toString();

    const parameters = new HttpParams()
      .append('offset', getExpertsRequest.offset.toString())
      .append('count', getExpertsRequest.count.toString())
      .append('isInHouse', checkParam(getExpertsRequest.isInHouse));

    return this.http.get<CollectionResponse<ExpertResponse>>(`${this.baseApiUrl}/experts`, {
      params: parameters
    }).toPromise();
  }

  public getAsync(address: string): Promise<ExpertResponse> {
    return this.http.get<ExpertResponse>(`${this.baseApiUrl}/experts/${address}`).toPromise();
  }

  public createAsync(expertRequest: ExpertRequest) {
    return this.http.post(`${this.baseApiUrl}/experts/`, expertRequest).toPromise();
  }

  public updateAsync(updateRequest: ExpertUpdateRequest) {
    return this.http.put(this.baseApiUrl + '/experts/', updateRequest).toPromise();
  }

  public deleteAsync(deleteRequest: ExpertDeleteRequest) {
    const queryString = '?address=' + deleteRequest.address + '&transactionHash=' + deleteRequest.transactionHash;
    return this.http.delete(this.baseApiUrl + '/experts/' + queryString).toPromise();
  }

  public getAvailabilityStatusAsync(): Promise<ExpertAvailabilityStatusResponse> {
    return this.http.get<ExpertAvailabilityStatusResponse>(`${this.baseApiUrl}/experts/availability`).toPromise();
  }

  public async switchAvailabilityAsync(transactionHash, value) {
    await this.http.put(this.baseApiUrl + '/experts/availability', {
      transactionHash: transactionHash,
      value: value
    }).toPromise();
  }
}
