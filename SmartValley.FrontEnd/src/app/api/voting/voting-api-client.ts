import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {VotingResponse} from './voting-response';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class VotingApiClient extends BaseApiClient {

  constructor(private http: HttpClient) {
    super();
  }
  public async getLastSprintAsync(): Promise<VotingResponse> {
    return await this.http.get<VotingResponse>(this.baseApiUrl + '/votings/last').toPromise();
  }
}
