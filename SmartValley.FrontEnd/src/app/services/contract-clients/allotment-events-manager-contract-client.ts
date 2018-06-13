import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class AllotmentEventsManagerContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3Service: Web3Service,
              private userContext: UserContext,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contract = await this.contractClient.getAllotmentEventsManagerContract();
    this.abi = contract.abi;
    this.address = contract.address;
  }

  public async createAsync(eventId: number): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.create(
      eventId,
      {
        from: fromAddress
      });
  }
}
