import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ProjectManagerContractResponse} from './project-manager-contract-response';
import {ProjectContractResponse} from './project-contract-response';

@Injectable()
export class ContractApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getProjectManagerContractAsync(): Promise<ProjectManagerContractResponse> {
    return await this.http.get<ProjectManagerContractResponse>(this.baseApiUrl + '/contracts/projectManager')
      .toPromise();
  }

  async getProjectContractAsync(): Promise<ProjectContractResponse> {
    return await this.http.get<ProjectContractResponse>(this.baseApiUrl + '/contracts/project')
      .toPromise();
  }

  async getTokenContractAsync(): Promise<ProjectManagerContractResponse> {
    return await this.http.get<ProjectManagerContractResponse>(this.baseApiUrl + '/contracts/token')
      .toPromise();
  }
}
