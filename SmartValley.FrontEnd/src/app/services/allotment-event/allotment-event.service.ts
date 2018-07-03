import {Injectable} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventsManagerContractClient} from '../contract-clients/allotment-events-manager-contract-client';
import {AllotmentEventContractClient} from '../contract-clients/allotment-event-contract-client.service';
import {AllotmentEvent} from './allotment-event';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {GetAllotmentEventsRequest} from '../../api/allotment-events/request/get-allotment-events-request';
import {SmartValleyTokenContractClient} from '../contract-clients/smart-valley-token-contract-client.service';
import BigNumber from 'bignumber.js';
import {CollectionResponse} from '../../api/collection-response';
import {TokenBalance} from './token-balance';

@Injectable()
export class AllotmentEventService {

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private allotmentEventContractClient: AllotmentEventContractClient,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
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
    allotmentEvents.items = allotmentEventsResponse.items.map(i => AllotmentEvent.create(i)).filter(i => i.eventContractAddress);
    const balances = await this.getTokensBalancesAsync(allotmentEvents.items.map(i => i.id));
    allotmentEvents.items.map(event => {
      event.totalTokens = balances.firstOrDefault(i => i.holderAddress === event.eventContractAddress).balance;
    });
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

  public async getTokensBalancesAsync(eventsIds: Array<number>): Promise<Array<TokenBalance>> {
    const response = await this.allotmentEventsApiClient.getTokensBalancesAsync(eventsIds);
    return response.items.map(i => TokenBalance.create(i));
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
    const transactionHash = await this.allotmentEventContractClient.receiveTokensAsync(eventAddress);
    await this.allotmentEventsApiClient.receiveTokensAsync(eventId, transactionHash);
    return transactionHash;
  }
}
