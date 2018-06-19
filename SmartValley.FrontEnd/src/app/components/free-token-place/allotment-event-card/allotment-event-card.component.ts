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
        return this.event.project.imageUrl || this.blockiesService.getImageForAddress(this.event.project.authorAddress);
    }

    public pad(n) {
        return (n < 10 ? '0' : '') + n;
    }

    public getAllotmentEventTimeLeft() {
        const eventDate = new Date(this.event.finishDate);
        const currentDate = new Date().getTime();

        let secondsLeft = (eventDate - currentDate) / 1000;

        const days = this.pad(parseInt(secondsLeft / 86400));
        secondsLeft = secondsLeft % 86400;

        const hours = this.pad(parseInt(secondsLeft / 3600));
        secondsLeft = secondsLeft % 3600;

        const minutes = this.pad(parseInt(secondsLeft / 60));
        const seconds = this.pad(parseInt(secondsLeft % 60));

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
