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

  constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
              private router: Router,
              private dialogService: DialogService) {
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
      this.loadAllotmentEventsAsync();
    }
  }

  public showStartAllotmentEventModal(allotmenEventData: AllotmentEventResponse) {
      this.dialogService.showStartAllotmentEventDialog(allotmenEventData);
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

}
