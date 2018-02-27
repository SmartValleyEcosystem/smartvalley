import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectResponse} from './project-response';
import {CollectionResponse} from '../collection-response';
import {ProjectDetailsResponse} from './project-details-response';
import {GetScoringProjectsRequest} from './get-scoring-projects-request';
import {MyProjectsItemResponse} from './my-projects-item-response';
import {ScoringProjectResponse} from './scoring-project-response';
import {ExpertiseArea} from '../scoring/expertise-area.enum';

@Injectable()
export class ProjectApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getScoredProjectsAsync(): Promise<CollectionResponse<ProjectResponse>> {
    return await this.http.get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/projects/scored')
      .toPromise();
  }

  async getDetailsByIdAsync(id: number): Promise<ProjectDetailsResponse> {
    return this.http.get<ProjectDetailsResponse>(this.baseApiUrl + '/projects?projectId=' + id)
      .toPromise();
  }

  async getScoringProjectsByCategoriesAsync(request: GetScoringProjectsRequest): Promise<CollectionResponse<ScoringProjectResponse>> {
    const statusesQuery = request.statuses.map(i => i.StatusId).join(',');
    const params = new HttpParams().append('queryStatuses', statusesQuery);

    return this.http.get<CollectionResponse<ScoringProjectResponse>>(this.baseApiUrl + '/projects/scoring', {params: params})
      .toPromise();
  }

  public async getForScoringAsync(expertiseArea: ExpertiseArea): Promise<CollectionResponse<ProjectResponse>> {
    const parameters = new HttpParams().append('expertiseArea', expertiseArea.toString());

    return this.http
      .get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/projects/forscoring', {params: parameters})
      .toPromise();
  }

  public async getMyProjectsAsync(): Promise<CollectionResponse<MyProjectsItemResponse>> {
    return this.http
      .get<CollectionResponse<MyProjectsItemResponse>>(this.baseApiUrl + '/projects/my')
      .toPromise();
  }
}
