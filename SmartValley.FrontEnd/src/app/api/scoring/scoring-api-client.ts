import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient, HttpParams} from '@angular/common/http';
import {StartProjectScoringRequest} from './start-project-scoring-request';
import {ScoringResponse} from './scoring-response';
import {UpdateScoringRequest} from './update-scoring-request';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async startAsync(projectId: number,
                          transactionHash: string): Promise<void> {
    const request = <StartProjectScoringRequest>{
      projectId: projectId,
      transactionHash: transactionHash
    };
    await this.http
      .post(this.baseApiUrl + '/scoring/start', request)
      .toPromise();
  }

  public async getByProjectIdAsync(projectId: number): Promise<ScoringResponse> {
    const parameters = new HttpParams().append('projectId', projectId.toString());
    return this.http
      .get<ScoringResponse>(this.baseApiUrl + '/scoring', {params: parameters})
      .toPromise();
  }

  public async finishAsync(scoringId: number): Promise<void> {
    await this.http.post(this.baseApiUrl + '/scoring/finish', <UpdateScoringRequest> {scoringId: scoringId})
      .toPromise();
  }

  public async reopenAsync(scoringId: number): Promise<void> {
    await this.http.post(this.baseApiUrl + '/scoring/reopen', <UpdateScoringRequest> {scoringId: scoringId})
      .toPromise();
  }
}
