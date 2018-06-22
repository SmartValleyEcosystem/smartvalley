import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AllotmentEventParticipateDialogData} from './allotment-event-participate-dialog-data';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';

@Component({
  selector: 'app-allotment-event-participate-modal',
  templateUrl: './allotment-event-participate-modal.component.html',
  styleUrls: ['./allotment-event-participate-modal.component.scss']
})
export class AllotmentEventParticipateModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEventParticipateDialogData,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private participateModalComponent: MatDialogRef<AllotmentEventParticipateModalComponent>) { }

  public newBet: number;
  public frozenTime: Date;

  async ngOnInit() {
    const today = new Date();
    const freezingDuration = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
    const nextMonth = today.setDate(today.getDate() + freezingDuration);
    this.frozenTime = new Date(nextMonth);
    this.data.myBet = this.data.myBet || 0;
  }

  public getComputedShare() {
      let myBid =  this.data.myBet;
      if (this.newBet) {
          myBid += this.newBet;
      }

      return (myBid / this.data.totalBet) * this.data.tokenBalance;
  }

  public submit() {
      this.participateModalComponent.close(this.newBet);
  }
}
