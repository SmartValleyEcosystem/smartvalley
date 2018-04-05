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

@Injectable()
export class ExpertApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getAreasAsync(): Promise<CollectionResponse<AreaResponse>> {
    return await this.http.get<CollectionResponse<AreaResponse>>(`${this.baseApiUrl}/experts/areas`).toPromise();
  }

  public async getExpertStatusAsync(address: string): Promise<GetExpertStatusResponse> {
    return await this.http.get<GetExpertStatusResponse>(`${this.baseApiUrl}/experts/${address}/status`).toPromise();
  }

  public async createApplicationAsync(request: CreateExpertApplicationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/experts/applications', request.body).toPromise();
  }

  public async getPendingApplicationsAsync(): Promise<CollectionResponse<PendingExpertApplicationsResponse>> {
    return await this.http.get<CollectionResponse<PendingExpertApplicationsResponse>>(`${this.baseApiUrl}/experts/applications`)
      .toPromise();
  }

  public async getApplicationByIdAsync(id: number): Promise<ExpertApplicationResponse> {
    return await this.http.get<ExpertApplicationResponse>(`${this.baseApiUrl}/experts/applications/${id}`).toPromise();
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

  public async getExpertsListAsync(offset: number, count: number): Promise<CollectionResponse<ExpertResponse>> {
    const parameters = new HttpParams()
      .append('offset', offset.toString())
      .append('count', count.toString());

    return await this.http.get<CollectionResponse<ExpertResponse>>(`${this.baseApiUrl}/experts/all/`, {
      params: parameters
    }).toPromise();
  }

  public async getAsync(address: string): Promise<ExpertResponse> {
    const parameters = new HttpParams()
      .append('address', address);

    return await this.http.get<ExpertResponse>(`${this.baseApiUrl}/experts/`, {
      params: parameters
    }).toPromise();
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
