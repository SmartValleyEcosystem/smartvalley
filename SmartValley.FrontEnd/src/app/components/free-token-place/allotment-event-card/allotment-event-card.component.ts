import {Component, OnInit, Input, OnDestroy, EventEmitter, Output} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';

@Component({
    selector: 'app-allotment-event-card',
    templateUrl: './allotment-event-card.component.html',
    styleUrls: ['./allotment-event-card.component.scss']
})
export class AllotmentEventCardComponent implements OnInit, OnDestroy {

    constructor(private router: Router,
                private blockiesService: BlockiesService) {
    }

    private timer: NodeJS.Timer;

    @Input() public event: AllotmentEventCard;
    @Input() public finished = false;
    @Output() finishEvent: EventEmitter<number> = new EventEmitter<number>();

    ngOnInit() {
        this.timer = <NodeJS.Timer>setInterval(async () => await this.getAllotmentEventTimeLeft(), 1000);
        this.event.timer = {
            days: '00',
            hours: '00',
            minutes: '00',
            seconds: '00'
        };
    }

    ngOnDestroy(): void {
        clearInterval(this.timer);
    }

    public getProjectLink(id) {
        return decodeURIComponent(
            this.router.createUrlTree([Paths.Project + '/' + id]).toString()
        );
    }

    public imageUrl(): string {
        return this.event.project.imageUrl || this.blockiesService.getImageForAddress(this.event.eventContractAddress);
    }

    public pad(n) {
        return (n < 10 ? '0' : '') + Math.floor(parseInt(n));
    }

    public getAllotmentEventTimeLeft() {
        const eventDate = new Date(this.event.finishDate);
        const currentDate = new Date().getTime();

        const eventDateNumber: number = eventDate.getTime();
        let secondsLeft = (eventDateNumber - currentDate) / 1000;

        const days = this.pad(secondsLeft / 86400);
        secondsLeft = secondsLeft % 86400;

        const hours = this.pad((secondsLeft / 3600));
        secondsLeft = secondsLeft % 3600;

        const minutes = this.pad(secondsLeft / 60);
        const seconds = this.pad(secondsLeft % 60);

        if (new Date() < eventDate) {
            this.event.timer.days = days;
            this.event.timer.hours = hours;
            this.event.timer.minutes = minutes;
            this.event.timer.seconds = seconds;
        } else {
            this.finishEvent.emit(this.event.id);
        }
    }
}
