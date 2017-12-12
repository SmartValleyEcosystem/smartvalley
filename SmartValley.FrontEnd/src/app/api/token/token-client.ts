import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {Web3Service} from '../../services/web3-service';
import {ContractApiClient} from '../contract/contract-api-client';

const unit = require('ethjs-unit');


@Injectable()
export class TokenClient extends BaseApiClient {

  private AbiOfContract: string;
  private contractAddress: string;

  constructor(private http: HttpClient,
              private web3: Web3Service,
              private contractClient: ContractApiClient) {
    super();
  }

  async loadContractInformationAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.AbiOfContract = tokenContract.abi;
    this.contractAddress = tokenContract.address;
  }

  async getTokenBalanceFromAddress(accountAddress: string): Promise<number> {
    const token = this.web3.getContract(this.AbiOfContract, this.contractAddress);
// suppose you want to call a function named myFunction of myContract
    const result = await token.balanceOf(accountAddress);
    const number = unit.fromWei(result[0].toString(10), 'ether');
    return number;
  }
}
