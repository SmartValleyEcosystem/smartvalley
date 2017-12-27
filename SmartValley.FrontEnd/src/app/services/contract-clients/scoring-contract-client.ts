import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {AuthenticationService} from '../authentication/authentication-service';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {Estimate} from '../estimate';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';

@Injectable()
export class ScoringContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractApiClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contract = await this.contractApiClient.getScoringContractAsync();
    this.abi = contract.abi;
    this.address = contract.address;
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
