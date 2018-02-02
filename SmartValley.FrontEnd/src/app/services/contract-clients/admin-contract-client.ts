import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';
import {isNullOrUndefined} from 'util';
import {AdminRequest} from '../../api/admin/admin-request';
import {Md5} from 'ts-md5';
import {AuthenticationService} from '../authentication/authentication-service';

@Injectable()
export class AdminContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getAdminRegisterContractAsync();
    this.abi = tokenContract.abi;
    this.address = tokenContract.address;
  }

  public async isAdminAsync(accountAddress: string): Promise<boolean> {
    const token = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractBoolValue(await token.isAdministrator(accountAddress));
  }

  public async addAdminAsync(accountAddress: string): Promise<number> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.authenticationService.getCurrentUser().account;
    return await contract.add(
      accountAddress,
      {from: fromAddress});
  }

  public async deleteAdminAsync(accountAddress: string): Promise<number> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.authenticationService.getCurrentUser().account;
    return await contract.remove(
      accountAddress,
      {from: fromAddress});
  }
}
