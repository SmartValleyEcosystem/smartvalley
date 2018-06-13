import {Injectable} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventsManagerContractClient} from '../contract-clients/allotment-events-manager-contract-client';

@Injectable()
export class AllotmentEventService {

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient) {

  }

  public async createAsync(name: string,
                           tokenContractAddress: string,
                           tokenDecimals: number,
                           tokenTicker: string,
                           projectId: number,
                           finishDate?: Date) {
    const response = await this.allotmentEventsApiClient.createAsync(name, tokenContractAddress, tokenDecimals, tokenTicker, projectId, finishDate);
    const transactionHash = await this.allotmentEventsManagerContractClient.createAsync(response.allotmentEventId);
    await this.allotmentEventsApiClient.publishAsync(response.allotmentEventId, transactionHash);
  }
}
