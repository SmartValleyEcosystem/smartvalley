import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ScoringApplicationResponse} from './scoring-application-response';
import {SaveScoringApplicationRequest} from './save-scoring-application-request';

@Injectable()
export class ScoringApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getScoringApplicationsAsync(projectId: number): Promise<ScoringApplicationResponse> {
    return this.http
      .get<ScoringApplicationResponse>(this.baseApiUrl + `/projects/${projectId}/scoring/applications/`)
      .toPromise();
  }

  public async saveScoringApplicationProjectAsync(projectId: number, requestData: SaveScoringApplicationRequest) {
    await this.http.post(this.baseApiUrl + `/projects/${projectId}/scoring/applications/`, requestData).toPromise();
  }
}
