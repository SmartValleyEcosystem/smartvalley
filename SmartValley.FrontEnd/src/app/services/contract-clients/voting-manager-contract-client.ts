import {ContractClient} from './contract-client';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {Web3Service} from '../web3-service';
import {Injectable} from '@angular/core';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class VotingManagerContractClient implements ContractClient {
  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getVotingManagerContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
  }

  public async enqueueProjectAsync(projectId: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.enqueueProject(
      projectId.replace(/-/g, ''),
      {from: fromAddress});
  }
}
