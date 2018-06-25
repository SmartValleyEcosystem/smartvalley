import {Injectable} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventsManagerContractClient} from '../contract-clients/allotment-events-manager-contract-client';

@Injectable()
export class AllotmentEventService {

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient) {
  }

  public async createAndPublishAsync(name: string,
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

    await this.publishAsync(response.allotmentEventId,
      name,
      tokenContractAddress,
      tokenDecimals,
      tokenTicker,
      finishDate);
  }

  public async publishAsync(allotmentEventId: number,
                            name: string,
                            tokenContractAddress: string,
                            tokenDecimals: number,
                            tokenTicker: string,
                            finishDate?: Date) {
    const transactionHash = await this.allotmentEventsManagerContractClient.createAsync(
      allotmentEventId,
      name,
      tokenDecimals,
      tokenTicker,
      tokenContractAddress,
      finishDate);

    await this.allotmentEventsApiClient.publishAsync(allotmentEventId, transactionHash);
  }

  public async startAsync(eventId: number) {
    const transactionHash = await this.allotmentEventsManagerContractClient.startAsync(eventId);
    await this.allotmentEventsApiClient.startAsync(eventId, transactionHash);
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

  public async participateAsync(eventId: number, amount: number) {
    const transactionHash = await this.allotmentEventsManagerContractClient.freezeAsync(amount);
    await this.allotmentEventsApiClient.participateAsync(eventId, transactionHash);
  }
}
