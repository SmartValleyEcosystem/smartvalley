import {ContractClient} from './contract-client';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {Web3Service} from '../web3-service';
import {BigNumber} from 'bignumber.js';
import {Injectable} from '@angular/core';

@Injectable()
export class ScoringContractClient implements ContractClient {
  abi: string;
  address: string;

  constructor(private contractClient: ContractApiClient,
              private web3Service: Web3Service) {
  }

  public async initializeAsync(): Promise<void> {
    const scoringContract = await this.contractClient.getScoringContractAsync();
    this.abi = scoringContract.abi;
  }

  public async getScoringCostAsync(scoringContractAddress: string): Promise<BigNumber> {
    const contract = this.web3Service.getContract(this.abi, scoringContractAddress);
    return await contract.getScoringCost();
  }
}
