import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectResponse} from './project-response';
import {CollectionResponse} from '../collection-response';
import {ProjectDetailsResponse} from './project-details-response';
import {GetScoringProjectsRequest} from './get-scoring-projects-request';
import {MyProjectsItemResponse} from './my-projects-item-response';
import {AreaType} from '../scoring/area-type.enum';
import {ScoringProjectResponse} from './scoring-project-response';
import {ScoredProject} from '../expert/scored-project';
import {SearchProjectResponse} from './search-projects-response';
import {CreateProjectRequest} from './create-project-request';

@Injectable()
export class ProjectApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async createAsync(request: CreateProjectRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/projects', request).toPromise();
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
    const statusesQuery = request.statuses.map(i => 'statuses=' + i.StatusId).join('&');

    return this.http.get<CollectionResponse<ScoringProjectResponse>>(this.baseApiUrl + '/projects/scoring/?' + statusesQuery)
      .toPromise();
  }

  public async getForScoringAsync(areaType: AreaType): Promise<CollectionResponse<ProjectResponse>> {
    const parameters = new HttpParams().append('expertiseArea', areaType.toString());

    return this.http
      .get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/projects/forscoring', {params: parameters})
      .toPromise();
  }

  public async getMyProjectsAsync(): Promise<CollectionResponse<MyProjectsItemResponse>> {
    return this.http
      .get<CollectionResponse<MyProjectsItemResponse>>(this.baseApiUrl + '/projects/my')
      .toPromise();
  }

  public getScoredProjectAsync(page: number, pageSize: number): Promise<CollectionResponse<ScoredProject>> {
    const parameters = new HttpParams().append('Page', page.toString())
      .append('PageSize', pageSize.toString());

    return this.http
      .get<CollectionResponse<ScoredProject>>(this.baseApiUrl + '/projects/scored', {params: parameters})
      .toPromise();
  }

  public getProjectsBySearchString(searchString: string): Promise<CollectionResponse<SearchProjectResponse>> {
    const parameters = new HttpParams().append('SearchString', searchString);

    return this.http
      .get<CollectionResponse<SearchProjectResponse>>(this.baseApiUrl + '/projects/search', {params: parameters})
      .toPromise();
  }
}
