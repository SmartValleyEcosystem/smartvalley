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
  }

  public inputString: string;
  public newBid: BigNumber;
  public frozenTime: Date;
  public changeParticipate = new Subject<any>();
  public isDescriptionShow = false;
  public computedShare: BigNumber;
  public isNewBidGreatherThanBalance = false;

  async ngOnInit() {
    const today = new Date();
    const freezingDuration = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
    const nextMonth = today.setDate(today.getDate() + freezingDuration);
    this.frozenTime = new Date(nextMonth);
  }

  public getComputedShare() {
    this.isNewBidGreatherThanBalance = false;
    if (this.inputString) {
      this.newBid = new BigNumber(this.inputString, 10).shift(this.data.svtDecimals);
    } else {
      this.newBid = new BigNumber(0, 10);
    }

    this.isDescriptionShow = false;

    let userTotalBid = this.data.existingUserBid.plus(this.newBid);
    let allotmentEventTotalBid = this.data.allotmentEventTotalBid.plus(this.newBid);
    let computedShare = userTotalBid.mul(this.data.tokenBalance).div(allotmentEventTotalBid);

    this.isDescriptionShow = true;

    this.computedShare = computedShare;
  }

  public submit() {
    if (!this.newBid && !this.inputString) {
      this.participateModalComponent.close();
      return;
    }
    if (this.newBid && this.newBid.greaterThan(this.data.userSvtBalance.shift(-this.data.svtDecimals))) {
      this.isNewBidGreatherThanBalance = true;
      return;
    }
    if (this.newBid) {
      this.participateModalComponent.close(this.newBid);
    }
  }
}
