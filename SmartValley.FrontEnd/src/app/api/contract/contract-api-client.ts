import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ContractResponse} from './contract-response';

@Injectable()
export class ContractApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getProjectManagerContractAsync(): Promise<ContractResponse> {
    return await this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/projectManager')
      .toPromise();
  }

  async getVotingManagerContractAsync(): Promise<ContractResponse> {
    return await this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/votingManager')
      .toPromise();
  }

  async getScoringContractAsync(): Promise<ContractResponse> {
    return await this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoring')
      .toPromise();
  }

  async getTokenContractAsync(): Promise<ContractResponse> {
    return await this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/token')
      .toPromise();
  }

  async getMinterContractAsync(): Promise<ContractResponse> {
    return await this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/minter')
      .toPromise();
  }
}
