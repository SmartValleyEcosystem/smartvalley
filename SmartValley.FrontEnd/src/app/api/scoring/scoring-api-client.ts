import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient, HttpParams} from '@angular/common/http';
import {ProjectForScorringResponse} from './project-for-scorring-response';
import {ProjecsForScorringRequest} from './projecs-for-scorring-request';

@Injectable()
export class ScoringApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getProjectForScoringAsync(request: ProjecsForScorringRequest): Promise<Array<ProjectForScorringResponse>> {
    return this.http.get<Array<ProjectForScorringResponse>>(this.baseApiUrl + '/scoring?category=' + request.scroringCategory ).toPromise();
  }
}
