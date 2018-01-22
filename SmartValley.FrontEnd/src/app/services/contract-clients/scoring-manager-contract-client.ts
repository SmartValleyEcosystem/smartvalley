import {Injectable} from '@angular/core';
import {AuthenticationService} from '../authentication/authentication-service';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {TokenContractClient} from './token-contract-client';
import {ContractClient} from './contract-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {Estimate} from '../estimate';

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
    return this.web3Service.fromWei(cost, await this.tokenClient.getTokenDecimalsAsync());
  }

  public async submitEstimatesAsync(projectAddress: string,
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
      const commentHash = await this.web3Service.getHashAsync(estimate.comments);
      commentHashes.push(commentHash);
    }

    return await contract.submitEstimates(
      projectAddress,
      <number>expertiseArea,
      questionIds,
      scores,
      commentHashes,
      {from: fromAddress});
  }
}
