import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {Application} from '../../services/application';

@Injectable()
export class ApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async createApplicationAsync(application: Application) {
    await this.http.post(this.baseApiUrl + '/applications', application).subscribe();
  }

  async getByProjectIdAsync(id: number) {
    return this.http.get<Application>(this.baseApiUrl + '/applications?projectId=' + id )
      .toPromise();
  }
}
