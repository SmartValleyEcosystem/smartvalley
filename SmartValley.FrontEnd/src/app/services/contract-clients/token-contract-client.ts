import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';
import {isNullOrUndefined} from 'util';

@Injectable()
export class TokenContractClient implements ContractClient {

  public abi: string;
  public address: string;

  private decimals: number = null;

  constructor(private web3: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.abi = tokenContract.abi;
    this.address = tokenContract.address;
  }

  public async getBalanceAsync(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.abi, this.address);
    const balance = ConverterHelper.extractNumberValue(await token.balanceOf(accountAddress));
    const decimals = await this.getDecimalsAsync();
    return this.web3.fromWei(balance, decimals);
  }

  public async getAvailableBalanceAsync(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.abi, this.address);
    const balance = ConverterHelper.extractNumberValue(await token.getAvailableBalance(accountAddress));
    const decimals = await this.getDecimalsAsync();
    return this.web3.fromWei(balance, decimals);
  }

  public async getDecimalsAsync(): Promise<number> {
    if (this.web3.isMetamaskInstalled && isNullOrUndefined(this.decimals)) {
      const token = this.web3.getContract(this.abi, this.address);
      const decimals = await token.decimals();
      this.decimals = ConverterHelper.extractNumberValue(decimals);
    }
    return this.decimals;
  }
}
