import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';

@Injectable()
export class TokenContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.abi = tokenContract.abi;
    this.address = tokenContract.address;
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.abi, this.address);
    const balance = ConverterHelper.extractNumberValue(await token.balanceOf(accountAddress));
    const decimals = ConverterHelper.extractNumberValue(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }

  async getTokenDecimalsAsync(): Promise<number> {
    const token = this.web3.getContract(this.abi, this.address);
    return ConverterHelper.extractNumberValue(await token.decimals());
  }

  async getAvailableBalanceAsync(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.abi, this.address);
    const balance = ConverterHelper.extractNumberValue(await token.getAvailableBalance(accountAddress));
    const decimals = ConverterHelper.extractNumberValue(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }
}
