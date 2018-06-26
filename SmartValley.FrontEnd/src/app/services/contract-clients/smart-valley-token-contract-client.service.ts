import {Injectable} from '@angular/core';
import {ContractClient} from './contract-client';
import {Web3Service} from '../web3-service';
import {UserContext} from '../authentication/user-context';
import {ContractApiClient} from '../../api/contract/contract-api-client';

@Injectable()
export class SmartValleyTokenContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contract = await this.contractClient.getSmartValleyTokenContractAsync();
    this.abi = contract.abi;
    this.address = contract.address;
  }
}
