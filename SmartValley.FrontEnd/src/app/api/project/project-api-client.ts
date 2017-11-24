import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectResponse} from './project-response';
import {CollectionResponse} from '../collection-response';
import {ProjectDetailsResponse} from './project-details-response';

@Injectable()
export class ProjectApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getScoredProjectsAsync(): Promise<CollectionResponse<ProjectResponse>> {
    return await this.http.get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/projects/scored')
      .toPromise();
  }

  async getDetailsByIdAsync(id: number) {
    return this.http.get<ProjectDetailsResponse>(this.baseApiUrl + '/projects?projectId=' + id )
      .toPromise();
  }

}
