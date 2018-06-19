import {Injectable} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertDeleteRequest} from '../../api/expert/expert-delete-request';
import {ExpertsRegistryContractClient} from '../contract-clients/experts-registry-contract-client';

@Injectable()
export class ExpertService {

  constructor(private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private expertApiClient: ExpertApiClient) {
  }

  public async deleteAsync(address: string): Promise<void> {
    const transactionHash = await this.expertsRegistryContractClient.removeAsync(address);
    await this.expertApiClient.deleteAsync(<ExpertDeleteRequest>{
      transactionHash: transactionHash,
      address: address
    });
  }
}
