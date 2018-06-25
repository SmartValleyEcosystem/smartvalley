import {Component, OnInit, Input, OnDestroy, EventEmitter, Output} from '@angular/core';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {DialogService} from '../../../services/dialog-service';
import {Balance} from '../../../services/balance/balance';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {AllotmentEventParticipateDialogData} from '../../common/allotment-event-participate-modal/allotment-event-participate-dialog-data';
import {UserContext} from '../../../services/authentication/user-context';
import {isNullOrUndefined} from 'util';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';

@Component({
  selector: 'app-allotment-event-card',
  templateUrl: './allotment-event-card.component.html',
  styleUrls: ['./allotment-event-card.component.scss']
})
export class AllotmentEventCardComponent implements OnInit, OnDestroy {

  constructor(private router: Router,
              private blockiesService: BlockiesService,
              private allotmentEventService: AllotmentEventService,
              private dialogService: DialogService,
              private userContext: UserContext,
              private authenticationService: AuthenticationService,
              private erc223ContractClient: Erc223ContractClient) {
  }

  private timer: NodeJS.Timer;
  public tokenBalance: number;
  public myBet: number;
  public totalBet: number;
  public canRecieveTokens: boolean;

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
    this.tokenBalance = await this.erc223ContractClient.getTokenBalanceAsync(this.event.tokenContractAddress, this.event.eventContractAddress);

    this.canRecieveTokens = !isNullOrUndefined(this.event.participants)
      || !this.event.participants.some(i => i.userId === this.userContext.getCurrentUser().id && i.isCollected) && this.finished;
  }

  ngOnDestroy(): void {
    clearInterval(this.timer);
  }

  public getPercentShare() {
    return (this.myBet * 100) / this.totalBet;
  }

  public async showParticipateDialogAsync() {
    if (await this.authenticationService.authenticateAsync()) {
      const participateResult = await this.dialogService.showParticipateDialog(<AllotmentEventParticipateDialogData>{
        balance: this.balance,
        totalBet: this.totalBet,
        myBet: this.myBet,
        tokenBalance: this.tokenBalance
      });
      if (participateResult) {
        await this.allotmentEventService.participateAsync(this.event.id, participateResult);
      }
    }
  }

  public imageUrl(): string {
    return this.event.project.imageUrl || this.blockiesService.getImageForAddress(this.event.eventContractAddress);
  }

  public pad(n) {
    return (n < 10 ? '0' : '') + Math.floor(parseInt(n));
  }

  public async showReceiveTokensModalAsync() {
    const myTokens = this.tokenBalance / this.totalBet * this.myBet;
    const result = await this.dialogService.showReceiveTokensModalAsync(this.tokenBalance, this.totalBet, this.myBet, myTokens, this.event.tokenTicker);
    if (result) {
      await this.allotmentEventService.receiveTokensAsync(this.event.id, this.event.eventContractAddress);
      this.canRecieveTokens = false;
    }
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
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
