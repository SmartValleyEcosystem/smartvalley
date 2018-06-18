import {Component, OnInit, Input} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';

@Component({
    selector: 'app-allotment-event-card',
    templateUrl: './allotment-event-card.component.html',
    styleUrls: ['./allotment-event-card.component.scss']
})
export class AllotmentEventCardComponent implements OnInit {

    constructor(private router: Router,
                private blockiesService: BlockiesService) {
    }

    @Input() public event: AllotmentEventCard;
    @Input() public finished = false;

    ngOnInit() {
    }

    public getProjectLink(id) {
        return decodeURIComponent(
            this.router.createUrlTree([Paths.Project + '/' + id]).toString()
        );
    }

    public imageUrl(): string {
        return this.event.project.imageUrl || this.blockiesService.getImageForAddress(this.event.project.authorAddress);
    }
}
