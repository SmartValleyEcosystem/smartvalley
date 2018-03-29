import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ScoringApplicationResponse} from './scoring-application-response';
import {SaveScoringApplicationRequest} from './save-scoringApplicationRequest';


@Injectable()
export class ScoringApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getScoringApplicationsAsync(projectId: number): Promise<ScoringApplicationResponse> {
    return this.http
      .get<ScoringApplicationResponse>(this.baseApiUrl + `/projects/${projectId}/scoringapplications/`)
      .toPromise();
  }

  public async saveScoringApplicationProjectAsync(projectId: number, requestData: SaveScoringApplicationRequest) {
    await this.http.post(this.baseApiUrl + `/projects/${projectId}/scoringapplications/`, {
      projectId: projectId,
      saveScoringApplicationRequest: requestData
    }).toPromise();
  }

  public async submitScoringApplicationProjectAsync(projectId: number) {
    await this.http.post(this.baseApiUrl + `/projects/${projectId}/scoringapplications/submit/`, {projectId: projectId}).toPromise();
  }
}
