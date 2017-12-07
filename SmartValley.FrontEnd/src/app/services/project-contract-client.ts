import {Injectable} from '@angular/core';
import {Web3Service} from './web3-service';
import {AuthenticationService} from './authentication-service';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {EstimateRequest} from '../api/estimates/estimate-request';

@Injectable()
export class ProjectContractClient {
  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service) {
  }

  public async submitEstimatesAsync(contractAddress: string,
                              abiString: string,
                              expertiseArea: ExpertiseArea,
                              estimates: Array<EstimateRequest>): Promise<string> {
    const projectContract = this.web3Service.getContract(abiString, contractAddress);
    const fromAddress = this.authenticationService.getCurrentUser().account;

    const questionIds: number[] = [];
    const scores: number[] = [];
    const commentHashes: string[] = [];

    for (const estimate of estimates) {
      questionIds.push(estimate.questionId);
      scores.push(estimate.score);
      const commentHash = await this.web3Service.getHash(estimate.comment);
      commentHashes.push(commentHash);
    }

    return await projectContract.submitEstimates(
      <number>expertiseArea,
      questionIds,
      scores,
      commentHashes,
      {from: fromAddress});
  }
}
