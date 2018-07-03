import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {UserContext} from '../authentication/user-context';
import {ScoringManagerContractClientBase} from './scoring-manager-contract-client-base';

@Injectable()
export class PrivateScoringManagerContractClient extends ScoringManagerContractClientBase {

  constructor(userContext: UserContext,
              web3Service: Web3Service,
              private contractClient: ContractApiClient) {
    super(userContext, web3Service);
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getPrivateScoringManagerContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
  }

  public async startAsync(projectExternalId: string,
                          areas: Array<number>,
                          expertAddresses: Array<string>): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;
    return await contract.start(
      projectExternalId.replace(/-/g, ''),
      areas,
      expertAddresses,
      {from: fromAddress});
  }
}
