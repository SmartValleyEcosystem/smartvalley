import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {StartProjectScoringRequest} from './start-project-scoring-request';
import {AreaRequest} from './area-request';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async startAsync(projectId: string,
                          areas: number[],
                          areaExpertCounts: number[],
                          transactionHash: string): Promise<void> {
    const request = <StartProjectScoringRequest>{
      projectExternalId: projectId,
      areas: areas.map((a, index) => <AreaRequest>{area: a, expertsCount: areaExpertCounts[index]}),
      transactionHash: transactionHash};
    await this.http
      .post(this.baseApiUrl + '/scoring/start', request)
      .toPromise();
  }
}
