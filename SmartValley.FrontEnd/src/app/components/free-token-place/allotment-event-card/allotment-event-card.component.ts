import {Component, OnInit, Input, OnDestroy, EventEmitter, Output} from '@angular/core';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {DialogService} from '../../../services/dialog-service';
import {Balance} from '../../../services/balance/balance';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {AllotmentEventParticipateDialogData} from '../../common/allotment-event-participate-modal/allotment-event-participate-dialog-data';
import {User} from '../../../services/authentication/user';
import {UserContext} from '../../../services/authentication/user-context';
import {isNullOrUndefined} from 'util';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {EthereumTransactionEntityTypeEnum} from '../../../api/transaction/ethereum-transaction-entity-type.enum';
import {TransactionRequest} from '../../../api/transaction/requests/transaction-request';
import {EthereumTransactionStatusEnum} from '../../../api/transaction/ethereum-transaction-status.enum';
import {EthereumTransactionTypeEnum} from '../../../api/transaction/ethereum-transaction-type.enum';
import {TransactionApiClient} from '../../../api/transaction/transaction-api-client';
import BigNumber from 'bignumber.js';

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
              private transactionApiClient: TransactionApiClient,
              private erc223ContractClient: Erc223ContractClient) {
  }

  private timer: NodeJS.Timer;
  public tokenBalance: BigNumber;
  public user: User;
  public userBid: BigNumber = new BigNumber(0);
  public userHasBid: boolean;
  public totalBid: BigNumber;
  public canRecieveTokens: boolean;

  @Input() public event: AllotmentEventCard;
  @Input() public finished = false;
  @Input() public balance: Balance;

  @Output() finishEvent: EventEmitter<number> = new EventEmitter<number>();

  async ngOnInit() {
    this.user = this.userContext.getCurrentUser();
    this.timer = <NodeJS.Timer>setInterval(async () => await this.getAllotmentEventTimeLeft(), 1000);
    if (this.user) {
        this.userHasBid = this.event.participants.some((a) => a.userId === this.user.id);

        if (this.userHasBid) {
          this.userBid = new BigNumber(this.event.participants.find((a) => a.userId === this.user.id).bid);
        }
    }

    this.totalBid = this.event.participants.reduce((sum, c) => new BigNumber(c.bid).plus(sum), new BigNumber(0));
    this.event.timer = {
        days: '00',
        hours: '00',
        minutes: '00',
        seconds: '00'
    };

    this.tokenBalance = await this.erc223ContractClient.getTokenBalanceAsync(this.event.tokenContractAddress, this.event.eventContractAddress);

    this.canRecieveTokens = !isNullOrUndefined(this.event.participants) && this.finished
        || !this.event.participants.some(i => i.userId === this.userContext.getCurrentUser().id && i.isCollected) && this.finished;
    this.getTransactionAsync();
  }

  ngOnDestroy(): void {
    clearInterval(this.timer);
  }

  public getActualShare() {
    const share = this.userBid.dividedBy(this.totalBid);
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getPotentialShare() {
    const balanceSVT = new BigNumber(this.balance.svt);
    const share = balanceSVT.dividedBy( balanceSVT.plus(this.totalBid) );
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getPercentShare() {
    const share = this.userBid.mul(100).dividedBy(this.totalBid);
    if (share.isNaN()) {
        return new BigNumber(0);
    }
    return share;
  }

  public async showParticipateDialogAsync() {
    if (await this.authenticationService.authenticateAsync()) {
      const participateResult = await this.dialogService.showParticipateDialog(<AllotmentEventParticipateDialogData>{
        balance: this.balance,
        totalBet: this.totalBid,
        myBet: this.userBid,
        tokenBalance: this.tokenBalance,
        decimals: this.event.tokenDecimals
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
    const myTokens = this.tokenBalance.dividedBy(this.totalBid).mul(this.userBid).toNumber();
    const result = await this.dialogService.showReceiveTokensModalAsync(this.tokenBalance, this.totalBid, this.userBid, myTokens, this.event.tokenTicker);
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

  public async getTransactionAsync() {
    if (!this.user) {
      return '';
    }
    const transactionInfo = await this.transactionApiClient.getEthereumTransactionAsync(<TransactionRequest>{
      count: 1,
      userIds: [this.user.id],
      entityIds: [this.event.id],
      entityTypes: [EthereumTransactionEntityTypeEnum.AllotmentEvent],
      statuses: [EthereumTransactionStatusEnum.InProgress]
    });
    if (transactionInfo.items[transactionInfo.items.length]) {
      this.event.transaction = transactionInfo.items[transactionInfo.items.length].hash;
      return;
    }
    this.event.transaction = '';
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
