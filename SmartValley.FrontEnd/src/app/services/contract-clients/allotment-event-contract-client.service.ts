import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {UserContext} from '../authentication/user-context';
import {Initializable} from '../initializable';

@Injectable()
export class AllotmentEventContractClient implements Initializable {

  private abi: string;

  constructor(private web3Service: Web3Service,
              private userContext: UserContext,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getAllotmentEventContractAsync();
    this.abi = contractResponse.abi;
  }

  public async receiveTokensAsync(eventAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, eventAddress);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.collectTokens({from: fromAddress});
  }
}
