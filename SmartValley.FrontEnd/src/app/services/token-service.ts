import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Web3Service} from './web3-service';
import {ContractApiClient} from '../api/contract/contract-api-client';

@Injectable()
export class TokenService {

  private contractAbi: string;
  private contractAddress: string;
  private isInitialized: boolean;

  constructor(private http: HttpClient,
              private web3: Web3Service,
              private contractClient: ContractApiClient) {
  }

  private async initilizeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.contractAbi = tokenContract.abi;
    this.contractAddress = tokenContract.address;
    this.isInitialized = true;
  }

  private extractValueFromContractResult(result: Array<any>): number {
    return +result[0].toString(10);
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.contractAbi, this.contractAddress);
    const balance = this.extractValueFromContractResult(await token.balanceOf(accountAddress));
    const decimals = this.extractValueFromContractResult(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }
}
