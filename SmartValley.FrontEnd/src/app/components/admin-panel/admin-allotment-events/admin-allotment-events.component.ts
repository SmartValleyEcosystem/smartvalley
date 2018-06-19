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
              private allotmentEventService: AllotmentEventService,
              private router: Router,
              private dialogService: DialogService,
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

  public async showNewAllotmentEventModalAsync(): Promise<void> {
    const allotmentEventCreated = await this.dialogService.showNewAllotmentEventDialog();
    if (allotmentEventCreated) {
      await this.loadAllotmentEventsAsync();
    }
  }

  public getTotalTokens(eventAddress: string): number | null {
    if (this.totalTokens.length === 0) {
      return null;
    }
    const total = this.totalTokens.firstOrDefault(i => i.key === eventAddress);
    if (isNullOrUndefined(total)) {
      return null;
    }
    return total.value;
  }

  public async showStartAllotmentEventModal(allotmentEventData: AllotmentEventResponse) {
    await this.dialogService.showStartAllotmentEventDialog(allotmentEventData);
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async showEditAllotmentModal(allotmentEvent: AllotmentEventResponse) {
    const editModal = await this.dialogService.showEditAllotmentEventDialog(allotmentEvent);
    if (editModal) {
      await this.allotmentEventService.editAsync(
        allotmentEvent.id,
        allotmentEvent.name,
        allotmentEvent.tokenContractAddress,
        allotmentEvent.tokenDecimals,
        allotmentEvent.tokenTicker,
        allotmentEvent.id,
        allotmentEvent.finishDate);
    }
  }
}
