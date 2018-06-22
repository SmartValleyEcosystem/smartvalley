import {Component, OnInit} from '@angular/core';
import {AllotmentEventsApiClient} from '../../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';
import {AllotmentEventStatus} from '../../../api/allotment-events/allotment-event-status';
import {LazyLoadEvent} from 'primeng/api';
import {DialogService} from '../../../services/dialog-service';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {GetAllotmentEventsRequest} from '../../../api/allotment-events/request/get-allotment-events-request';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {isNullOrUndefined} from 'util';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';

@Component({
  selector: 'app-admin-allotment-events',
  templateUrl: './admin-allotment-events.component.html',
  styleUrls: ['./admin-allotment-events.component.scss']
})
export class AdminAllotmentEventsComponent {

  public allotmentEvents: AllotmentEventResponse[];
  public selectedStatuses: AllotmentEventStatus[] = [];
  public AllotmentEventStatus = AllotmentEventStatus;
  public loading = true;
  public totalRecords: number;
  public offset = 0;
  public pageSize = 10;

  public totalTokens: { key: string, value: number }[] = [];

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private router: Router,
              private dialogService: DialogService,
              private allotmentEventService: AllotmentEventService,
              private erc223ContractClient: Erc223ContractClient) {
  }

  public async sortByStatusAsync(checked: boolean, status: AllotmentEventStatus): Promise<void> {
    if (checked) {
      this.selectedStatuses.push(status);
    } else {
      this.selectedStatuses = this.selectedStatuses.filter(s => s !== status);
    }
    await this.loadAllotmentEventsAsync();
  }

  private async loadAllotmentEventsAsync(): Promise<void> {
    this.loading = true;
    const getAllotmentEventsRequest = <GetAllotmentEventsRequest>{
      offset: this.offset,
      count: this.pageSize,
      statuses: this.selectedStatuses
    };
    const allotmentEventsRequest = await this.allotmentEventsApiClient.getAllotmentEvents(getAllotmentEventsRequest);
    this.allotmentEvents = allotmentEventsRequest.items;
    for (const event of this.allotmentEvents) {
      if (event.eventContractAddress === null) {
        continue;
      }
      const total = await this.erc223ContractClient.getTokenBalanceAsync(event.tokenContractAddress, event.eventContractAddress);
      this.totalTokens.push({key: event.eventContractAddress, value: total});
    }
    this.totalRecords = allotmentEventsRequest.totalCount;
    this.loading = false;
  }

  public async getAllotmentEventsAsync(event: LazyLoadEvent): Promise<void> {
    this.offset = event.first;
    await this.loadAllotmentEventsAsync();
  }

  public getTotalTokens(eventAddress: string): number {
    if (this.totalTokens.length === 0) {
      return 0;
    }
    const total = this.totalTokens.firstOrDefault(i => i.key === eventAddress);
    if (isNullOrUndefined(total)) {
      return 0;
    }
    return total.value;
  }

  public async showStartAllotmentEventModal(allotmenEventData: AllotmentEventResponse) {
    const start = await this.dialogService.showStartAllotmentEventDialog(allotmenEventData);
    if (start) {
      await this.allotmentEventService.startAsync(allotmenEventData.id);
    }
  }

  public async showNewAllotmentEventModalAsync(): Promise<void> {
    const allotmentEventCreated = await this.dialogService.showNewAllotmentEventDialog();
    if (allotmentEventCreated) {
      this.loadAllotmentEventsAsync();
    }
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async publishAsync(event: AllotmentEventResponse) {
    await this.allotmentEventService.publishAsync(event.id,
      event.name,
      event.tokenContractAddress,
      event.tokenDecimals,
      event.tokenTicker,
      event.finishDate);
  }

  public async showEditAllotmentModal(allotmentEvent: AllotmentEventResponse) {
    const editModal = await this.dialogService.showEditAllotmentEventDialog(allotmentEvent);
    if (editModal) {
      await this.allotmentEventService.editAsync(
        allotmentEvent.id,
        editModal.eventName,
        editModal.tokenAddress,
        editModal.tokenDecimals,
        editModal.ticker,
        allotmentEvent.projectId,
        editModal.finishDate);
    }
  }

  public async showReturnAddressModalAsync() {
    const address = await this.allotmentEventsManagerContractClient.getReturnAddressAsync();
    const newAddress = await this.dialogService.showReturnAddressDialogAsync(address);
    if (isNullOrUndefined(newAddress) || address === newAddress) {
      return;
    }
    await this.allotmentEventsManagerContractClient.setReturnAddressAsync(newAddress);
  }

  public async showFreezeTimeModalAsync() {
    const freezeTime = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
    const newFreezeTime = await this.dialogService.showSetFreezeTimeDialogAsync(freezeTime);
    if (isNullOrUndefined(newFreezeTime) || freezeTime === newFreezeTime) {
      return;
    }
    await this.allotmentEventsManagerContractClient.setFreezingDurationAsync(newFreezeTime);
  }
}
