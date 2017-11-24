import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectsForScoringRequest} from './projecs-for-scoring-request';
import {CollectionResponse} from '../collection-response';
import {ProjectResponse} from '../project/project-response';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getProjectForScoringAsync(request: ProjectsForScoringRequest): Promise<CollectionResponse<ProjectResponse>> {
    return this.http.get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/scoring?category=' + request.scoringCategory )
      .toPromise();
  }

  async getMyProjectsAsync(): Promise<CollectionResponse<ProjectResponse>> {
    return this.http.get<CollectionResponse<ProjectResponse>>(this.baseApiUrl + '/scoring/myprojects')
      .toPromise();
  }
}
