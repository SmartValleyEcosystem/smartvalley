import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';
import {isNullOrUndefined} from 'util';
import {AdminRequest} from '../../api/admin/admin-request';
import {Md5} from 'ts-md5';
import {AuthenticationService} from '../authentication/authentication-service';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class AdminContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private userContext: UserContext) {
  }

  public async initializeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getAdminRegistryContractAsync();
    this.abi = tokenContract.abi;
    this.address = tokenContract.address;
  }

  public async addAsync(accountAddress: string): Promise<string> {
    const fromAddress = await this.userContext.getCurrentUser().account;
    const contract = this.web3Service.getContract(this.abi, this.address);
    return await contract.add(
      accountAddress,
      {from: fromAddress});
  }

  public async deleteAsync(accountAddress: string, fromAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    return await contract.remove(
      accountAddress,
      {from: fromAddress});
  }
}
