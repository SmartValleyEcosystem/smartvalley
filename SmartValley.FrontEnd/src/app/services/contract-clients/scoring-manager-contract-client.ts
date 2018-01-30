import {Injectable} from '@angular/core';
import {AuthenticationService} from '../authentication/authentication-service';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {TokenContractClient} from './token-contract-client';
import {ContractClient} from './contract-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {Estimate} from '../estimate';
import {Md5} from 'ts-md5';

@Injectable()
export class ScoringManagerContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private tokenClient: TokenContractClient) {
  }

  public async initializeAsync(): Promise<void> {
    const scoringManagerContract = await this.contractClient.getScoringManagerContractAsync();
    this.abi = scoringManagerContract.abi;
    this.address = scoringManagerContract.address;
  }

  public startAsync(projectId: string): Promise<string> {
    const scoringManagerContract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.authenticationService.getCurrentUser().account;

    return scoringManagerContract.start(
      projectId.replace(/-/g, ''),
      {from: fromAddress});
  }

  public async getScoringCostAsync(): Promise<number> {
    const scoringManager = this.web3Service.getContract(this.abi, this.address);
    const cost = ConverterHelper.extractNumberValue(await scoringManager.scoringCostWEI());
    return this.web3Service.fromWei(cost, await this.tokenClient.getDecimalsAsync());
  }

  public async submitEstimatesAsync(scoringAddress: string,
                                    expertiseArea: ExpertiseArea,
                                    estimates: Array<Estimate>): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.authenticationService.getCurrentUser().account;

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
      <number>expertiseArea,
      questionIds,
      scores,
      commentHashes,
      {from: fromAddress});
  }
}
