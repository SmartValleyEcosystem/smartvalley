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

  public getPrivateScoringManagerContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/privateScoringManager').toPromise();
  }

  public getScoringOffersManagerContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoringOffersManager').toPromise();
  }

  public getAdminRegistryContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/adminRegistry').toPromise();
  }

  public getExpertRegistryContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/expertsRegistry').toPromise();
  }

  public getERC223ContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/erc223').toPromise();
  }

  public getAllotmentEventContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/allotmentEvents').toPromise();
  }

  public getAllotmentEventsManagerContract(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/allotmentEventsManager').toPromise();
  }

  public getScoringParametersProviderContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/scoringParametersProvider').toPromise();
  }

  public getSmartValleyTokenContractAsync(): Promise<ContractResponse> {
    return this.http.get<ContractResponse>(this.baseApiUrl + '/contracts/smartValleyToken').toPromise();
  }
}
