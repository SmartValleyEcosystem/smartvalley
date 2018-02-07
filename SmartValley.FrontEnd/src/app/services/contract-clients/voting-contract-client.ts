import {ContractClient} from './contract-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {Injectable} from '@angular/core';
import {ConverterHelper} from '../converter-helper';
import {TokenContractClient} from './token-contract-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class VotingContractClient implements ContractClient {
  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private tokenContractClient: TokenContractClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getVotingContractAsync();
    this.abi = contractResponse.abi;
  }

  public async submitVoteAsync(votingSprintAddress: string, projectExternalId: string, amount: number): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, votingSprintAddress);
    const fromAddress = this.userContext.getCurrentUser().account;

    const token = this.web3Service.getContract(this.tokenContractClient.abi, this.tokenContractClient.address);
    const decimals = ConverterHelper.extractNumberValue(await token.decimals());
    const amountWIthDecimals = this.web3Service.toWei(amount, decimals);
    return contract.submitVote(
      projectExternalId.replace(/-/g, ''),
      amountWIthDecimals,
      {from: fromAddress});
  }
}
