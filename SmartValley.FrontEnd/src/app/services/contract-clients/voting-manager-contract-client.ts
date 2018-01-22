import {ContractClient} from './contract-client';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {Web3Service} from '../web3-service';
import {AuthenticationService} from '../authentication/authentication-service';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../../api/base-api-client';
import {VotingResponse} from '../../services/contract-clients/voting-response';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class VotingManagerContractClient extends BaseApiClient implements ContractClient {
  public abi: string;
  public address: string;

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private http: HttpClient) {
    super();
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getVotingManagerContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
  }

  public async enqueueProjectAsync(projectId: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.authenticationService.getCurrentUser().account;

    return contract.enqueueProject(
      projectId.replace(/-/g, ''),
      {from: fromAddress});
  }

  public async getLastSprintAsync(): Promise<VotingResponse> {
    return await this.http.get<VotingResponse>(this.baseApiUrl + '/votings/last').toPromise();
  }
}
