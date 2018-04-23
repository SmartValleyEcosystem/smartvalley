import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ContractResponse} from './contract-response';

@Injectable()
export class ContractApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getScoringManagerContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoringManager').toPromise();
  }

  public getScoringContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoring').toPromise();
  }

  public getScoringExpertsManagerContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoringExpertsManager').toPromise();
  }

  public getAdminRegistryContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/adminRegistry').toPromise();
  }

  public getExpertRegistryContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/expertsRegistry').toPromise();
  }
}
