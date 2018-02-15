import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {GetExpertStatusResponse} from './get-expert-status-response';
import {ExpertApplicationRequest} from './expert-applitacion-request';


@Injectable()
export class ExpertApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getExpertStatusAsync(address: string): Promise<GetExpertStatusResponse> {
    return await this.http.get<GetExpertStatusResponse>(`${this.baseApiUrl}/experts/${address}/status`).toPromise();
  }

  public async createApplicationAsync(request: ExpertApplicationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/experts/application', request.body).toPromise();
  }
}
