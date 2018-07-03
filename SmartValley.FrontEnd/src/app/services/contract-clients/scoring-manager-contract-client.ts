import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {UserContext} from '../authentication/user-context';
import {ScoringManagerContractClientBase} from './scoring-manager-contract-client-base';

@Injectable()
export class ScoringManagerContractClient extends ScoringManagerContractClientBase {

  constructor(userContext: UserContext,
              web3Service: Web3Service,
              private contractClient: ContractApiClient) {
    super(userContext, web3Service);
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getScoringManagerContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
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
}
