import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {StartProjectScoringRequest} from './start-project-scoring-request';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async startAsync(projectId: string, transactionHash: string): Promise<void> {
    const request = <StartProjectScoringRequest>{projectExternalId: projectId, transactionHash: transactionHash};
    await this.http
      .post(this.baseApiUrl + '/scoring/start', request)
      .toPromise();
  }
}
