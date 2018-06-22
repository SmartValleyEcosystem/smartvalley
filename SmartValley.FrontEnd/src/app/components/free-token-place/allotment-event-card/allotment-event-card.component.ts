import {Component, OnInit, Input, OnDestroy, EventEmitter, Output} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {DialogService} from '../../../services/dialog-service';
import {Balance} from '../../../services/balance/balance';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {AllotmentEventsApiClient} from '../../../api/allotment-events/allotment-events-api-client';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';
import {AllotmentEventParticipateDialogData} from '../../common/allotment-event-participate-modal/allotment-event-participate-dialog-data';

@Component({
    selector: 'app-allotment-event-card',
    templateUrl: './allotment-event-card.component.html',
    styleUrls: ['./allotment-event-card.component.scss']
})
export class AllotmentEventCardComponent implements OnInit, OnDestroy {

    constructor(private router: Router,
                private blockiesService: BlockiesService,
                private allotmentEventsApiClient: AllotmentEventsApiClient,
                private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
                private dialogService: DialogService,
                private allotmentEventService: AllotmentEventService,
                private authenticationService: AuthenticationService,
                private erc223ContractClient: Erc223ContractClient) {
    }

    private timer: NodeJS.Timer;
    public tokenBalance: number;
    public myBet: number;
    public totalBet: number;

    @Input() public event: AllotmentEventCard;
    @Input() public finished = false;
    @Input() public balance: Balance;

    @Output() finishEvent: EventEmitter<number> = new EventEmitter<number>();

    async ngOnInit() {
        this.timer = <NodeJS.Timer>setInterval(async () => await this.getAllotmentEventTimeLeft(), 1000);
        this.myBet = 60000;
        this.totalBet = 174599;
        this.event.timer = {
            days: '00',
            hours: '00',
            minutes: '00',
            seconds: '00'
        };
        this.tokenBalance = await this.erc223ContractClient
          .getTokenBalanceAsync(this.event.tokenContractAddress, this.event.eventContractAddress);
    }

    ngOnDestroy(): void {
        clearInterval(this.timer);
    }

    public getProjectLink(id) {
        return decodeURIComponent(
            this.router.createUrlTree([Paths.Project + '/' + id]).toString()
        );
    }

    public getPercentShare() {
        return (this.myBet * 100) / this.totalBet;
    }

    public async showParticipateDialogAsync(balance: Balance, event: AllotmentEventCard) {
        if (await this.authenticationService.authenticateAsync()) {
            const participateResult = await this.dialogService.showParticipateDialog(<AllotmentEventParticipateDialogData>{
                balance: balance,
                totalBet: this.totalBet,
                myBet: this.myBet,
                tokenBalance: this.tokenBalance
            });
            if (participateResult) {
                const transactionHash = await this.allotmentEventsManagerContractClient.freezeAsync(participateResult);
                await this.allotmentEventsApiClient.participateAsync(event.id, transactionHash);
            }
        }
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
