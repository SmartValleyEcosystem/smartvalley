import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Web3Service} from './web3-service';
import {ContractApiClient} from '../api/contract/contract-api-client';
import {isNullOrUndefined} from 'util';

const unit = require('ethjs-unit');


@Injectable()
export class TokenService {

  private contractAbi: string;
  private contractAddress: string;

  constructor(private http: HttpClient,
              private web3: Web3Service,
              private contractClient: ContractApiClient) {
  }

  private async checkContractAsync(): Promise<void> {
    if (isNullOrUndefined(this.contractAbi) || isNullOrUndefined(this.contractAddress)) {
      await this.initilizeAsync();
    }
  }

  private async initilizeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.contractAbi = tokenContract.abi;
    this.contractAddress = tokenContract.address;
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    await this.checkContractAsync();
    const token = this.web3.getContract(this.contractAbi, this.contractAddress);
    const balance = await token.balanceOf(accountAddress);
    return unit.fromWei(balance[0].toString(10), 'ether');
  }
}
