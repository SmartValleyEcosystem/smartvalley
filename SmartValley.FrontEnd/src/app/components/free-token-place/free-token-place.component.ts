import {Component, OnDestroy, OnInit} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {GetAllotmentEventsRequest} from '../../api/allotment-events/request/get-allotment-events-request';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {AllotmentEventCard} from './allotment-event-card';
import {ProjectQuery} from '../../api/project/project-query';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {SortDirection} from '../../api/sort-direction.enum';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectResponse} from '../../api/project/project-response';

@Component({
    selector: 'app-free-token-place',
    templateUrl: './free-token-place.component.html',
    styleUrls: ['./free-token-place.component.scss']
})
export class FreeTokenPlaceComponent implements OnInit {

    public allotmentEvents: AllotmentEventCard[];
    public activeEvents: AllotmentEventCard[] = [];
    public finishedEvents: AllotmentEventCard[] = [];
    public selectedStatuses: AllotmentEventStatus[] = [];
    public offset = 0;
    public pageSize = 10;
    public projects: ProjectResponse[] = [];

    constructor(private allotmentEventsApiClient: AllotmentEventsApiClient,
                private projectApiClient: ProjectApiClient) {
    }

    async ngOnInit() {
        await this.loadAllotmentEventsAsync();
    }

    private async loadAllotmentEventsAsync(): Promise<void> {
        const allotmentEvents = await this.allotmentEventsApiClient.getAllotmentEvents(<GetAllotmentEventsRequest>{
            offset: this.offset,
            count: this.pageSize,
            statuses: this.selectedStatuses
        });

        this.allotmentEvents = allotmentEvents.items;

        const projectIds: number[] = [];

        this.allotmentEvents.map((a) => {
            projectIds.push(a.projectId);
        });

        const projectResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
            offset: 0,
            count: projectIds.length,
            onlyScored: false,
            orderBy: ProjectsOrderBy.CreationDate,
            direction: SortDirection.Descending,
            projectIds: projectIds
        });

        this.projects = projectResponse.items;

        this.allotmentEvents.map( (a, i) => {
            const currentEvent = this.allotmentEvents[i];
            a['project'] = this.projects.find((p) => p.id === currentEvent.projectId );
            a['timer'] = {
                days: '00',
                hours: '00',
                minutes: '00',
                seconds: '00'
            };
            if ( a.status === AllotmentEventStatus.Finished ) {
                this.finishedEvents.push(currentEvent);
            }else {
                this.activeEvents.push(currentEvent);
            }
        });
    }

    public finishEvent(id: number) {
        this.finishedEvents.push(   this.activeEvents.find((a) => a.id === id )   );
        this.activeEvents = this.activeEvents.filter( (a, i) => {
          return a.id !== id;
        };
    }
}
