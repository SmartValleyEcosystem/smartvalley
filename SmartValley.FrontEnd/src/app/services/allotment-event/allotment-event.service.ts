import {Injectable} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventsManagerContractClient} from '../contract-clients/allotment-events-manager-contract-client';
import {AllotmentEventsContractClient} from '../contract-clients/allotment-events-contract-client';
import {AllotmentEvent} from './allotment-event';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {GetAllotmentEventsRequest} from '../../api/allotment-events/request/get-allotment-events-request';
import {Erc223ContractClient} from '../contract-clients/erc223-contract-client';
import {SmartValleyTokenContractClient} from '../contract-clients/smart-valley-token-contract-client.service';
import BigNumber from 'bignumber.js';
import {CollectionResponse} from '../../api/collection-response';

@Injectable()
export class AllotmentEventService {

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private allotmentEventsContractClient: AllotmentEventsContractClient,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private erc223ContractClient: Erc223ContractClient,
              private smartValleyTokenContractClient: SmartValleyTokenContractClient) {
  }

  public async getAllotmentEventsAsync(offset: number, count: number, statuses: AllotmentEventStatus[]): Promise<CollectionResponse<AllotmentEvent>> {
    const getAllotmentEventsRequest = <GetAllotmentEventsRequest>{
      offset: offset,
      count: count,
      statuses: statuses
    };
    const allotmentEventsResponse = await this.allotmentEventsApiClient.getAllotmentEventsAsync(getAllotmentEventsRequest);
    const allotmentEvents = {
      items: [],
      totalCount: 0
    };
    allotmentEvents.items = allotmentEventsResponse.items.map(i => AllotmentEvent.create(i));
    for (const event of allotmentEvents.items) {
      if (event.eventContractAddress) {
        event.totalTokens = await this.erc223ContractClient.getTokenBalanceAsync(event.tokenContractAddress, event.eventContractAddress);
      }
    }
    allotmentEvents.totalCount = allotmentEventsResponse.totalCount;
    return allotmentEvents;
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

  public async removeAsync(eventId: number) {
    const transactionHash = await this.allotmentEventsManagerContractClient.removeAsync(eventId);
    await this.allotmentEventsApiClient.removeAsync(eventId, transactionHash);
  }

  public async participateAsync(eventId: number, eventContractAddress: string, amount: BigNumber): Promise<string> {
    const transactionHash = await this.smartValleyTokenContractClient.freezeAsync(amount, eventContractAddress);
    await this.allotmentEventsApiClient.participateAsync(eventId, transactionHash);
    return transactionHash;
  }

  public async receiveTokensAsync(eventId: number, eventAddress: string): Promise<string> {
    const transactionHash = await this.allotmentEventsContractClient.receiveTokensAsync(eventAddress);
    await this.allotmentEventsApiClient.receiveTokensAsync(eventId, transactionHash);
    return transactionHash;
  }
}
