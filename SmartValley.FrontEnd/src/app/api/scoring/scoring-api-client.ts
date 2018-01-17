import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectsForScoringRequest} from './projecs-for-scoring-request';
import {CollectionResponse} from '../collection-response';
import {ProjectResponse} from '../project/project-response';
import {HttpClient, HttpParams} from '@angular/common/http';
import {StartProjectScoringRequest} from './start-project-scoring-request';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getProjectForScoringAsync(request: ProjectsForScoringRequest): Promise<CollectionResponse<ProjectResponse>> {
    const parameters = new HttpParams()
      .append('expertiseArea', request.expertiseArea.toString());

    return this.http
      .get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/scoring', {params: parameters})
      .toPromise();
  }

  public async getMyProjectsAsync(): Promise<CollectionResponse<ProjectResponse>> {
    return this.http
      .get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/scoring/myprojects')
      .toPromise();
  }

  public async startAsync(projectId: string, transactionHash: string): Promise<void> {
    const request = <StartProjectScoringRequest>{projectExternalId: projectId, transactionHash: transactionHash};
    await this.http
      .post(this.baseApiUrl + '/scoring/start', request)
      .toPromise();
  }
}
