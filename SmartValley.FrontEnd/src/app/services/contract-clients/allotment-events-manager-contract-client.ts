import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';
import {isNullOrUndefined} from 'util';
import * as moment from 'moment';

@Injectable()
export class AllotmentEventsManagerContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3Service: Web3Service,
              private userContext: UserContext,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contract = await this.contractClient.getAllotmentEventsManagerContract();
    this.abi = contract.abi;
    this.address = contract.address;
  }

  public async createAsync(eventId: number,
                           name: string,
                           tokenDecimals: number,
                           tokenTicker: string,
                           tokenContractAddress: string,
                           finishDate?: Date): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.create(
      eventId,
      name,
      isNullOrUndefined(tokenDecimals) ? 0 : tokenDecimals,
      tokenTicker,
      tokenContractAddress,
      this.isDateEmpty(finishDate) ? 0 : Math.floor(moment(finishDate).valueOf() / 1000),
      {from: fromAddress});
  }

  public async editAsync(eventId: number,
                         name: string,
                         tokenDecimals: number,
                         tokenTicker: string,
                         tokenContractAddress: string,
                         finishDate?: Date): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.edit(
      eventId,
      name,
      isNullOrUndefined(tokenDecimals) ? 0 : tokenDecimals,
      tokenTicker,
      tokenContractAddress,
      this.isDateEmpty(finishDate) ? 0 : Math.floor(moment(finishDate).valueOf() / 1000),
      {from: fromAddress});
  }

  public async startAsync(eventId: number): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.start(eventId, {from: fromAddress});
  }

  private isDateEmpty(date?: Date): boolean {
    return isNullOrUndefined(date) || date.toString().length === 0;
  }
}
