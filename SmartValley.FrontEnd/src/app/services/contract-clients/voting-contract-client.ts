import {ContractClient} from './contract-client';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {Injectable} from '@angular/core';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class VotingContractClient implements ContractClient {
  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getVotingContractAsync();
    this.abi = contractResponse.abi;
  }

  public async submitVoteAsync(votingSprintAddress: string, projectExternalId: string, amount: number): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, votingSprintAddress);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.submitVote(
      projectExternalId.replace(/-/g, ''),
      0,
      {from: fromAddress});
  }
}
