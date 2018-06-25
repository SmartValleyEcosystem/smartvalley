import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class AllotmentEventsContractClient {

  public abi: string;

  constructor(private web3Service: Web3Service,
              private userContext: UserContext,
              private contractClient: ContractApiClient) {
  }

  public async receiveTokensAsync(eventAddress: string): Promise<string> {
    const eventContract = await this.contractClient.getAllotmentEventContractAsync();
    const contract = this.web3Service.getContract(eventContract.abi, eventAddress);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.collect({from: fromAddress});
  }
}
