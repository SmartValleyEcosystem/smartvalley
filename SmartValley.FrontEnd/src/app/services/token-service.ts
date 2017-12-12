import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Web3Service} from './web3-service';
import {ContractApiClient} from '../api/contract/contract-api-client';

const unit = require('ethjs-unit');


@Injectable()
export class TokenService {

  private contractAbi: string;
  private contractAddress: string;

  constructor(private http: HttpClient,
              private web3: Web3Service,
              private contractClient: ContractApiClient) {
  }

  async loadContractInformationAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.contractAbi = tokenContract.abi;
    this.contractAddress = tokenContract.address;
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.contractAbi, this.contractAddress);
    const result = await token.balanceOf(accountAddress);
    const number = unit.fromWei(result[0].toString(10), 'ether');
    return number;
  }
}
