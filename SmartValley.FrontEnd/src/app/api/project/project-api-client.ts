import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ScoredProjectResponse} from './scored-project-response';

@Injectable()
export class ProjectApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getScoredProjectsAsync(): Promise<ScoredProjectResponse[]> {
    return await this.http.get<ScoredProjectResponse[]>(this.baseApiUrl + '/project/scored')
      .toPromise();
  }
}
