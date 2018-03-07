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
import {PendingExpertListResponse} from './pending-expert-list-response';
import {NewExpertRequest} from './new-expert-request';
import {DeleteExpertRequest} from './delete-expert-request';
import {EditExpertRequest} from './edit-expert-request';
import {ExpertScoring} from './expert-scoring';
import {ExpertHistoryOffer} from './expert-history-offer';
import {Observable} from 'rxjs/Observable';
import {of} from 'rxjs/observable/of';

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

  public async getExpertsListAsync(page: number, pageSize: number): Promise<CollectionResponse<PendingExpertListResponse>> {
    const parameters = new HttpParams()
      .append('page', page.toString())
      .append('pageSize', pageSize.toString());

    return await this.http.get<CollectionResponse<PendingExpertListResponse>>(`${this.baseApiUrl}/experts/`, {
      params: parameters
    }).toPromise();
  }

  public async createNewExpertsAsync(createExpertData: NewExpertRequest) {
    await this.http.post(`${this.baseApiUrl}/experts/`, createExpertData).toPromise();
  }

  public async deleteExpertAsync(deleteExpertData: DeleteExpertRequest) {
    const queryString = '?address=' + deleteExpertData.address + '&transactionHash=' + deleteExpertData.transactionHash;
    await this.http.delete(this.baseApiUrl + '/experts/' + queryString).toPromise();
  }

  public async editExpertAsync(editExpertData: EditExpertRequest) {
    await this.http.put(this.baseApiUrl + '/experts/', editExpertData).toPromise();
  }
}
