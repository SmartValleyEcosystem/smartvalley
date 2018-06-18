import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectResponse} from './project-response';
import {CollectionResponse} from '../collection-response';
import {GetScoringProjectsRequest} from './get-scoring-projects-request';
import {MyProjectResponse} from './my-project-response';
import {ScoringProjectResponse} from '../scoring-application/scoring-project-response';
import {CreateProjectRequest} from './create-project-request';
import {ProjectQuery} from './project-query';
import {isNullOrUndefined} from 'util';
import {UpdateProjectRequest} from './update-project-request';
import {ProjectSummaryResponse} from './project-summary-response';
import {ProjectAboutResponse} from './project-about-response';
import {AddProjectTeamMemberPhotoRequest} from './add-project-team-member-photo-request';
import {AddProjectImageRequest} from './add-project-image-request';

@Injectable()
export class ProjectApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async createAsync(request: CreateProjectRequest): Promise<ProjectAboutResponse> {
    return this.http.post<ProjectAboutResponse>(this.baseApiUrl + '/projects', request).toPromise();
  }

  public async updateAsync(request: UpdateProjectRequest): Promise<ProjectAboutResponse> {
    return this.http.put<ProjectAboutResponse>(`${this.baseApiUrl}/projects/${request.id}/`, request).toPromise();
  }

  public async uploadTeamMemberPhotoAsync(request: AddProjectTeamMemberPhotoRequest): Promise<void> {
    await this.http.put(`${this.baseApiUrl}/projects/teammembers`, request.body).toPromise();
  }

  public async uploadProjectImageAsync(request: AddProjectImageRequest): Promise<void> {
    await this.http.put(`${this.baseApiUrl}/projects/${request.body.get('id')}/image`, request.body).toPromise();
  }

  public async deleteProjectImageAsync(id: number): Promise<void> {
    await this.http.delete(`${this.baseApiUrl}/projects/${id}/image`).toPromise();
  }

  public async deleteTeamMemberPhotoAsync(projectId: number, id: number): Promise<void> {
    await this.http.delete(`${this.baseApiUrl}/projects/${projectId}/teammembers/${id}`).toPromise();
  }

  public async deleteAsync(projectId: number): Promise<void> {
    await this.http.delete(`${this.baseApiUrl}/projects/${projectId}/`).toPromise();
  }

  async getScoringProjectsByCategoriesAsync(request: GetScoringProjectsRequest): Promise<CollectionResponse<ScoringProjectResponse>> {
    const statusesQuery = request.statuses.map(i => 'statuses=' + i.StatusId).join('&');

    return this.http.get<CollectionResponse<ScoringProjectResponse>>(this.baseApiUrl + '/projects/scoring/?' + statusesQuery)
      .toPromise();
  }

  public async getMyProjectAsync(): Promise<MyProjectResponse> {
    return this.http
      .get<MyProjectResponse>(this.baseApiUrl + '/projects/my')
      .toPromise();
  }

  public getAsync(query: ProjectQuery): Promise<CollectionResponse<ProjectResponse>> {
    const checkParam = (param) => isNullOrUndefined(param) ? '' : param.toString();

    let parameters = new HttpParams()
      .append('offset', query.offset.toString())
      .append('count', query.count.toString())
      .append('onlyScored', query.onlyScored.toString())
      .append('searchString', checkParam(query.searchString))
      .append('stageType', checkParam(query.stageType))
      .append('countryCode', checkParam(query.countryCode))
      .append('category', checkParam(query.categoryType))
      .append('minimumScore', checkParam(query.minimumScore))
      .append('maximumScore', checkParam(query.maximumScore))
      .append('orderBy', checkParam(query.orderBy))
      .append('isPrivate', checkParam(query.isPrivate))
      .append('sortDirection', checkParam(query.direction));

    if (query.scoringStatuses) {
      query.scoringStatuses.forEach(id => {
        parameters = parameters.append('scoringStatuses', id.toString());
      });
    }

    if (query.projectIds) {
      query.projectIds.forEach(id => {
        parameters = parameters.append('projectIds', id.toString());
      });
    }

    return this.http
      .get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/projects', {params: parameters})
      .toPromise();
  }

  public async getProjectSummaryAsync(projectId: number): Promise<ProjectSummaryResponse> {
    return this.http
      .get<ProjectSummaryResponse>(this.baseApiUrl + `/projects/${projectId}`)
      .toPromise();
  }

  public async getProjectAboutAsync(projectId: number): Promise<ProjectAboutResponse> {
    return this.http
      .get<ProjectAboutResponse>(this.baseApiUrl + `/projects/${projectId}/about`)
      .toPromise();
  }

}
