import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AllotmentEventParticipateDialogData} from './allotment-event-participate-dialog-data';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';
import BigNumber from 'bignumber.js';
import {Subject} from 'rxjs/Subject';

@Component({
  selector: 'app-allotment-event-participate-modal',
  templateUrl: './allotment-event-participate-modal.component.html',
  styleUrls: ['./allotment-event-participate-modal.component.scss']
})
export class AllotmentEventParticipateModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEventParticipateDialogData,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private participateModalComponent: MatDialogRef<AllotmentEventParticipateModalComponent>) {
    this.changeParticipate
      .map(event => event.target.value)
      .debounceTime(500)
      .subscribe(val => this.getComputedShare());
    this.data.myBet = new BigNumber(this.data.myBet) || new BigNumber(0);
    this.userSVT = this.data.balance.svt.toFormat();
  }

  public newBet: BigNumber;
  public frozenTime: Date;
  public changeParticipate = new Subject<any>();
  public isDescriptionShow = false;
  public computedShare: BigNumber;
  public userSVT: string;

  async ngOnInit() {
    const today = new Date();
    const freezingDuration = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
    const nextMonth = today.setDate(today.getDate() + freezingDuration);
    this.frozenTime = new Date(nextMonth);
  }

  public getComputedShare() {
    let myBid: BigNumber = this.data.myBet;
    this.isDescriptionShow = false;
    this.newBet = this.newBet ? new BigNumber(this.newBet) : new BigNumber(0);

    if (this.newBet) {
      myBid = myBid.plus(this.newBet);
    }

    let computedShare = myBid.dividedBy(this.data.totalBet).mul(this.data.tokenBalance);

    if (computedShare.isNaN()) {
      computedShare = new BigNumber(0);
    }

    this.isDescriptionShow = true;
    if (!computedShare.isFinite()) {
      computedShare = new BigNumber(this.data.tokenBalance);
    }

    this.computedShare = computedShare;
  }

  public submit() {
    this.participateModalComponent.close(this.newBet);
  }
}
