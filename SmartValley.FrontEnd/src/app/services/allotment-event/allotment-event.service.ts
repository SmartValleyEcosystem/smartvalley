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
    const response = await this.allotmentEventsApiClient.createAsync(
      name,
      tokenContractAddress,
      tokenDecimals,
      tokenTicker,
      projectId,
      finishDate);

    const transactionHash = await this.allotmentEventsManagerContractClient.createAsync(
      response.allotmentEventId,
      name,
      tokenDecimals,
      tokenTicker,
      tokenContractAddress,
      finishDate);

    await this.allotmentEventsApiClient.publishAsync(response.allotmentEventId, transactionHash);
  }

  public async editAsync(eventId: number,
                         name: string,
                         tokenContractAddress: string,
                         tokenDecimals: number,
                         tokenTicker: string,
                         projectId: number,
                         finishDate?: Date) {
    const transactionHash = await this.allotmentEventsManagerContractClient.editAsync(
      eventId,
      name,
      tokenDecimals,
      tokenTicker,
      tokenContractAddress,
      finishDate);

    await this.allotmentEventsApiClient.updateAsync(eventId, transactionHash);
  }
}
