import {Component, OnDestroy, OnInit} from '@angular/core';
import {AllotmentEventsApiClient} from '../../api/allotment-events/allotment-events-api-client';
import {GetAllotmentEventsRequest} from '../../api/allotment-events/request/get-allotment-events-request';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {AllotmentEventResponse} from '../../api/allotment-events/responses/allotment-event-response';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {AllotmentEventCard} from './allotment-event-card';
import {c} from '@angular/core/src/render3';

@Component({
    selector: 'app-free-token-place',
    templateUrl: './free-token-place.component.html',
    styleUrls: ['./free-token-place.component.scss']
})
export class FreeTokenPlaceComponent implements OnInit, OnDestroy {

    public allotmentEvents: AllotmentEventCard[];
    public activeEvents: AllotmentEventCard[] = [];
    public finishedEvents: AllotmentEventCard[] = [];
    public selectedStatuses: AllotmentEventStatus[] = [];
    public offset = 0;
    public pageSize = 10;
    private timer: NodeJS.Timer;

    constructor(private allotmentEventsApiClient: AllotmentEventsApiClient) {
    }

    async ngOnInit() {
        await this.loadAllotmentEventsAsync();
        this.timer = <NodeJS.Timer>setInterval(async () => await this.getEventTimeLeft(), 1000);
    }

    ngOnDestroy(): void {
        clearInterval(this.timer);
    }

    private async loadAllotmentEventsAsync(): Promise<void> {
        const getAllotmentEventsRequest = <GetAllotmentEventsRequest>{
            offset: this.offset,
            count: this.pageSize,
            statuses: this.selectedStatuses
        };
        const allotmentEventsRequest = await this.allotmentEventsApiClient.getAllotmentEvents(getAllotmentEventsRequest);
        this.allotmentEvents = allotmentEventsRequest.items;

        for (let i = 0; this.allotmentEvents.length > i; i++) {
            const currentEvent = this.allotmentEvents[i];
            currentEvent['timer'] = {
                days: '00',
                hours: '00',
                minutes: '00',
                seconds: '00'
            };
            if ( currentEvent.status === AllotmentEventStatus.Finished ) {
                this.finishedEvents.push(currentEvent);
            }else {
                this.activeEvents.push(currentEvent);
            }
        }
    }

    public pad(n) {
        return (n < 10 ? '0' : '') + n;
    }

    public getEventTimeLeft() {
        for (let i = 0; this.activeEvents.length > i; i++) {
            const eventDate = new Date(this.activeEvents[i].finishDate);
            const currentDate = new Date().getTime();

            let secondsLeft = (eventDate - currentDate) / 1000;

            const days = this.pad(parseInt(secondsLeft / 86400));
            secondsLeft = secondsLeft % 86400;

            const hours = this.pad(parseInt(secondsLeft / 3600));
            secondsLeft = secondsLeft % 3600;

            const minutes = this.pad(parseInt(secondsLeft / 60));
            const seconds = this.pad(parseInt(secondsLeft % 60));

            if (new Date() < eventDate) {
                this.activeEvents[i].timer.days = days;
                this.activeEvents[i].timer.hours = hours;
                this.activeEvents[i].timer.minutes = minutes;
                this.activeEvents[i].timer.seconds = seconds;
            }else {
                this.finishedEvents.push(this.activeEvents[i]);
                this.activeEvents.splice(i, 1);
            }
        }
    }
}
