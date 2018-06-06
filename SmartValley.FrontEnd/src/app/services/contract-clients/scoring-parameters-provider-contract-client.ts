import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';
import {BigNumber} from 'bignumber.js';
import {AreaType} from '../../api/scoring/area-type.enum';

@Injectable()
export class ScoringParametersProviderContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contract = await this.contractClient.getScoringParametersProviderContractAsync();
    this.abi = contract.abi;
    this.address = contract.address;
  }

  public async getAreaRewardAsync(areaType: AreaType): Promise<BigNumber> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    return await contract.getAreaReward(+areaType);
  }

  public async setAreaRewardsAsync(areas: Array<number>, costsWei: Array<BigNumber>): Promise<string> {
    const fromAddress = this.userContext.getCurrentUser().account;
    const contract = this.web3Service.getContract(this.abi, this.address);
    return await contract.setAreaRewards(areas, costsWei, {from: fromAddress});
  }
}
