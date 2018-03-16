import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {ContractClient} from './contract-client';
import {AreaType} from '../../api/scoring/area-type.enum';
import {Estimate} from '../estimate';
import {Md5} from 'ts-md5';
import {UserContext} from '../authentication/user-context';

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

  public async startAsync(projectId: string,
                          areas: Array<number>,
                          areaExpertCounts: Array<number>): Promise<string> {
    const scoringManagerContract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    const scoringCost = await this.getScoringCostAsync();

    return await scoringManagerContract.start(
      projectId.replace(/-/g, ''),
      areas,
      areaExpertCounts,
      {
        from: fromAddress,
        value: scoringCost
      });
  }

  public startForFreeAsync(projectId: string,
                           sprintAddress: string,
                           areas: Array<number>,
                           areaExpertCounts: Array<number>): Promise<string> {
    const scoringManagerContract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return scoringManagerContract.startForFree(
      projectId.replace(/-/g, ''),
      sprintAddress,
      areas,
      areaExpertCounts,
      {from: fromAddress});
  }

  public async getScoringCostAsync(): Promise<number> {
    const scoringManager = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractNumberValue(await scoringManager.scoringCostWEI());
  }

  public async submitEstimatesAsync(scoringAddress: string,
                                    areaType: AreaType,
                                    estimates: Array<Estimate>): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    const questionIds: number[] = [];
    const scores: number[] = [];
    const commentHashes: string[] = [];

    for (const estimate of estimates) {
      questionIds.push(estimate.questionId);
      scores.push(estimate.score);
      const commentHash = '0x' + Md5.hashStr(estimate.comments, false).toString();
      commentHashes.push(commentHash);
    }

    return await contract.submitEstimates(
      scoringAddress,
      <number>areaType,
      questionIds,
      scores,
      commentHashes,
      {from: fromAddress});
  }
}
