import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {GetExpertStatusResponse} from './get-expert-status-response';
import {CreateExpertApplicationRequest} from './expert-applitacion-request';
import {CollectionResponse} from '../collection-response';
import {PendingExpertApplicationsResponse} from './pending-expert-applications-response';


@Injectable()
export class ExpertApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
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
}
