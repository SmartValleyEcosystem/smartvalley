import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {GetLastSprintResponse} from './get-last-sprint-response';

@Injectable()
export class VotingApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getLastVotingSprintAsync(): Promise<GetLastSprintResponse> {
    return await this.http.get<GetLastSprintResponse>(this.baseApiUrl + '/votings/last')
      .toPromise();
  }
}
