import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectManagerContractResponse} from './project-manager-contract-response';

@Injectable()
export class ContractApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getProjectManagerContractAsync(): Promise<ProjectManagerContractResponse> {
    return await this.http.get<ProjectManagerContractResponse>(this.baseApiUrl + '/contracts/projectManager')
      .toPromise();
  }
}
