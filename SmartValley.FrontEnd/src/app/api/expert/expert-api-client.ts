import {HttpClient} from '@angular/common/http';
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

  public async acceptExpertApplicationAsync(id: number, areasToAccept: number[]) {
    await this.http.post(`${this.baseApiUrl}/experts/applications/${id}/accept`, <AcceptExpertApplicationRequest>{
      areas: areasToAccept
    }).toPromise();
  }

  public async rejectExpertApplicationAsync(id: number, reason: string) {
    await this.http.post(`${this.baseApiUrl}/experts/applications/${id}/reject`, <RejectApplicationRequest>{
      reason: reason
    }).toPromise();
  }
}
