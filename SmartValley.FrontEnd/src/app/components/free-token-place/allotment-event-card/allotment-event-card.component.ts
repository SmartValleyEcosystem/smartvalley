import {Component, OnInit, Input, OnDestroy, EventEmitter, Output} from '@angular/core';
import {BlockiesService} from '../../../services/blockies-service';
import {AllotmentEventCard} from '../allotment-event-card';
import {DialogService} from '../../../services/dialog-service';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {AllotmentEventParticipateDialogData} from '../../common/allotment-event-participate-modal/allotment-event-participate-dialog-data';
import {User} from '../../../services/authentication/user';
import {UserContext} from '../../../services/authentication/user-context';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {EthereumTransactionEntityTypeEnum} from '../../../api/transaction/ethereum-transaction-entity-type.enum';
import {TransactionRequest} from '../../../api/transaction/requests/transaction-request';
import {EthereumTransactionStatusEnum} from '../../../api/transaction/ethereum-transaction-status.enum';
import {TransactionApiClient} from '../../../api/transaction/transaction-api-client';
import BigNumber from 'bignumber.js';
import {UserBalance} from '../../../services/balance/user-balance';

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
              private transactionApiClient: TransactionApiClient) {
  }

  private timer: NodeJS.Timer;
  public user: User;
  public userBid: BigNumber = new BigNumber(0);
  public userHasBid = false;
  public canReceiveTokens: boolean;
  public potentialShare: BigNumber = new BigNumber(0);
  public potentialPercentShare: BigNumber = new BigNumber(100);
  public actualShare: BigNumber;
  public percentShare: BigNumber;

  @Input() public model: AllotmentEventCard;
  @Input() public finished = false;

  @Output() finishEvent: EventEmitter<number> = new EventEmitter<number>();

  async ngOnInit() {
    this.potentialShare = this.model.event.totalTokens;

    this.timer = <NodeJS.Timer>setInterval(async () => await this.getAllotmentEventTimeLeft(), 1000);
    if (this.authenticationService.isAuthenticated()) {
      const userBalance = new UserBalance(this.model.balance);
      this.user = this.userContext.getCurrentUser();
      this.userHasBid = this.model.event.userHasBid(this.user.id);
      this.userBid = this.model.event.getUserBid(this.user.id);
      this.actualShare = this.model.event.getActualShare(this.user.id);
      this.percentShare = this.model.event.getPercentShare(this.user.id);
      this.potentialShare = this.model.event.getPotentialShare(userBalance.actualSVTbalance);
      this.potentialPercentShare = this.model.event.getPotentialPercentShare(userBalance.actualSVTbalance);
      this.canReceiveTokens = this.finished && this.userHasBid && !this.model.event.isCollected(this.user.id);
      await this.loadUserTransactionsAsync();
    }
  }

  ngOnDestroy(): void {
    clearInterval(this.timer);
  }

  public async showParticipateDialogAsync() {
    if (await this.authenticationService.authenticateAsync()) {
      const participateResult = await this.dialogService.showParticipateDialogAsync(<AllotmentEventParticipateDialogData>{
        allotmentEventTotalBid: this.model.event.totalBid,
        existingUserBid: this.userBid,
        tokenBalance: this.model.event.totalTokens,
        tokenDecimals: this.model.event.tokenDecimals,
        svtDecimals: this.model.balance.svtDecimals,
        tokenTicker: this.model.event.tokenTicker,
        actualSVTbalance: new UserBalance(this.model.balance).actualSVTbalance
      });
      if (participateResult) {
        this.model.transaction = await this.allotmentEventService.participateAsync(this.model.event.id,
          this.model.event.eventContractAddress, participateResult);
      }
    }
  }

  public imageUrl(): string {
    return this.model.project.imageUrl || this.blockiesService.getImageForAddress(this.model.event.eventContractAddress);
  }

  private pad(n: number) {
    return (n < 10 ? '0' : '') + Math.floor(n);
  }

  public async showReceiveTokensModalAsync() {
    const result = await this.dialogService.showReceiveTokensModalAsync(
      this.model.event.totalTokens,
      this.model.event.totalBid,
      this.userBid,
      this.model.event.getUserTokens(this.user.id, this.model.event.totalTokens),
      this.model.event.tokenTicker,
      this.model.event.tokenDecimals,
      this.model.balance.svtDecimals
    );
    if (result) {
      this.model.transaction =
        await this.allotmentEventService.receiveTokensAsync(this.model.event.id, this.model.event.eventContractAddress);
      this.canReceiveTokens = false;
    }
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async loadUserTransactionsAsync() {
    const transactionInfo = await this.transactionApiClient.getEthereumTransactionsAsync(<TransactionRequest>{
      count: 1,
      userIds: [this.user.id],
      entityIds: [this.model.event.id],
      entityTypes: [EthereumTransactionEntityTypeEnum.AllotmentEvent],
      statuses: [EthereumTransactionStatusEnum.InProgress]
    });
    if (transactionInfo.items[transactionInfo.items.length - 1]) {
      this.model.transaction = transactionInfo.items[transactionInfo.items.length - 1].hash;
      return;
    }
    this.model.transaction = '';
  }

  public getAllotmentEventTimeLeft() {
    const eventDate = new Date(this.model.event.finishDate);
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
      this.model.timer.days = days;
      this.model.timer.hours = hours;
      this.model.timer.minutes = minutes;
      this.model.timer.seconds = seconds;
    } else {
      this.finishEvent.emit(this.model.event.id);
    }
  }
}
