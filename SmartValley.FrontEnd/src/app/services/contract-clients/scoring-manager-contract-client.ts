import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {AreaType} from '../../api/scoring/area-type.enum';
import {Estimate} from '../estimate';
import {Md5} from 'ts-md5';
import {UserContext} from '../authentication/user-context';
import {BigNumber} from 'bignumber.js';

@Injectable()
export class ScoringManagerContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const scoringManagerContract = await this.contractClient.getScoringManagerContractAsync();
    this.abi = scoringManagerContract.abi;
    this.address = scoringManagerContract.address;
  }

  public async startAsync(projectExternalId: string,
                          areas: Array<number>,
                          areaExpertCounts: Array<number>,
                          scoringCostEth: number): Promise<string> {
    const scoringManagerContract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await scoringManagerContract.start(
      projectExternalId.replace(/-/g, ''),
      areas,
      areaExpertCounts,
      {
        from: fromAddress,
        value: this.web3Service.toWei(scoringCostEth)
      });
  }

  public async startPrivateAsync(projectExternalId: string,
                          areas: Array<number>,
                          expertAddresses: Array<string>): Promise<string> {
    const scoringManagerContract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await scoringManagerContract.startPrivate(
      projectExternalId.replace(/-/g, ''),
      areas,
      expertAddresses,
      {
        from: fromAddress
      });
  }

  public async getScoringCostInAreaAsync(areaType: AreaType): Promise<BigNumber> {
    const scoringManager = this.web3Service.getContract(this.abi, this.address);
    return await scoringManager.estimateRewardsInAreaMap(+areaType);
  }

  public async setEstimateRewardsAsync(areas: Array<number>, costsWei: Array<BigNumber>): Promise<string> {
    const fromAddress = this.userContext.getCurrentUser().account;
    const scoringManager = this.web3Service.getContract(this.abi, this.address);
    return await scoringManager.setEstimateRewards(areas, costsWei, {from: fromAddress});
  }

  public async submitEstimatesAsync(projectExternalId: string,
                                    areaType: AreaType,
                                    conclusion: string,
                                    estimates: Array<Estimate>): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    const scoringCriteriaIds: number[] = [];
    const scores: number[] = [];
    const commentHashes: string[] = [];

    for (const estimate of estimates) {
      scoringCriteriaIds.push(estimate.scoringCriterionId);
      scores.push(estimate.score);
      const commentHash = '0x' + Md5.hashStr(estimate.comments, false).toString();
      commentHashes.push(commentHash);
    }

    const conclusionHash = '0x' + Md5.hashStr(conclusion, false).toString();
    return await contract.submitEstimates(
      projectExternalId.replace(/-/g, ''),
      <number>areaType,
      conclusionHash,
      scoringCriteriaIds,
      scores,
      commentHashes,
      {from: fromAddress});
  }
}
