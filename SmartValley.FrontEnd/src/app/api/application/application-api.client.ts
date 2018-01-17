import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitApplicationRequest} from './submit-application-request';

@Injectable()
export class ApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async submitAsync(request: SubmitApplicationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/applications', request).toPromise();
  }
}
