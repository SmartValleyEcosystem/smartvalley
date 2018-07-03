import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {AreaType} from '../../api/scoring/area-type.enum';
import {Estimate} from '../estimate';
import {Md5} from 'ts-md5';
import {UserContext} from '../authentication/user-context';
import {Initializable} from '../initializable';

@Injectable()
export abstract class ScoringManagerContractClientBase implements Initializable {

  protected abi: string;
  protected address: string;

  protected constructor(protected userContext: UserContext,
                        protected web3Service: Web3Service) {
  }

  abstract initializeAsync(): Promise<void>;

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
