import { Component, OnInit } from '@angular/core';
import {AllotmentEventsApiClient} from '../../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';
import {AllotmentEventStatus} from '../../../api/allotment-events/allotment-event-status';
import {LazyLoadEvent} from 'primeng/api';

@Component({
  selector: 'app-admin-allotment-events',
  templateUrl: './admin-allotment-events.component.html',
  styleUrls: ['./admin-allotment-events.component.scss']
})
export class AdminAllotmentEventsComponent implements OnInit {

  public allotmentEvents: AllotmentEventResponse[];
  public selectedStatuses: boolean[] = [];
  public loading = true;
  public totalRecords: number;
  public offset = 0;
  public pageSize = 10;

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient) { }

  async ngOnInit() {
    await this.loadAllotmentEventsAsync();
    this.prepareStatuses();
  }

  public async sortByStatus(event, status) {
      this.selectedStatuses[status] = event;
      await this.loadAllotmentEventsAsync();
  }

  private async loadAllotmentEventsAsync(): Promise<void> {
      this.loading = true;
      const getAllotmentEventsRequest = {
          offset: this.offset,
          count: this.pageSize,
          status: this.selectedStatuses
      };
      const allotmentEventsRequest = await this.allotmentEventsApiClient.getAllotmentEvents(getAllotmentEventsRequest);
      this.allotmentEvents = allotmentEventsRequest.items;
      this.totalRecords = allotmentEventsRequest.totalCount;
      this.loading = false;
  }

  public async getAllotmentEvents(event: LazyLoadEvent) {
        this.offset = event.first;
        await this.loadAllotmentEventsAsync();
  }

  public prepareStatuses() {
      const statusesCount = Object.keys(AllotmentEventStatus).length / 2;
      for (let i = 0; i < statusesCount; i++) {
        this.selectedStatuses[i] = false;
      }
  }

}
