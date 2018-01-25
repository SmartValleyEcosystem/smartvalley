import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {GetCurrentSprintResponse} from './get-current-sprint-response';

@Injectable()
export class VotingApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getCurrentVotingSprintAsync(): Promise<GetCurrentSprintResponse> {
    return await this.http.get<GetCurrentSprintResponse>(this.baseApiUrl + '/votings/current')
      .toPromise();
  }
}
