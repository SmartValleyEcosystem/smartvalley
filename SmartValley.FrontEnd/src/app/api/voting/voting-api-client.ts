import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {GetSprintResponse} from './get-current-sprint-response';
import {VotingSprintResponse} from './voting-sprint-response';

@Injectable()
export class VotingApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getCurrentVotingSprintAsync(): Promise<GetSprintResponse> {
    return await this.http.get<GetSprintResponse>(this.baseApiUrl + '/votings/current')
      .toPromise();
  }

  async getVotingSprintByAddressAsync(address: string): Promise<GetSprintResponse> {
    return await this.http.get<GetSprintResponse>(this.baseApiUrl + '/votings/' + address)
      .toPromise();
  }
}
