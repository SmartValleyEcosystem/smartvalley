import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {Application} from '../../services/application';
import {ProjectDetailsResponse} from '../project/project-details-response';

@Injectable()
export class ApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async createApplicationAsync(application: Application): Promise<void> {
    await this.http.post(this.baseApiUrl + '/applications', application).toPromise();
  }
}
