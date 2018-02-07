import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class MinterContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3Service: Web3Service,
              private userContext: UserContext,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const minterContract = await this.contractClient.getMinterContractAsync();
    this.abi = minterContract.abi;
    this.address = minterContract.address;
  }

  public async canGetTokensAsync(accountAddress: string): Promise<boolean> {
    const token = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractBoolValue(await token.canGetTokens(accountAddress));
  }

  public async getReceivingIntervalInDaysAsync(): Promise<number> {
    const token = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractNumberValue(await token.DAYS_INTERVAL_BETWEEN_RECEIVE());
  }

  public async getReceiveDateForAddressAsync(accountAddress: string): Promise<number> {
    const token = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractNumberValue(await token.receiversDateMap(accountAddress));
  }

  public async getTokensAsync(): Promise<string> {
    const accountAddress = this.userContext.getCurrentUser().account;
    const minter = this.web3Service.getContract(this.abi, this.address);
    return await minter.getTokens({from: accountAddress});
  }
}
