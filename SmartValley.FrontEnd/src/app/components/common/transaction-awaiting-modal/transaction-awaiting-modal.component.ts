import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {TransactionAwaitingModalData} from './transaction-awaiting-modal-data';

@Component({
  selector: 'app-project-creating-modal',
  templateUrl: './transaction-awaiting-modal.component.html',
  styleUrls: ['./transaction-awaiting-modal.component.css']
})
export class TransactionAwaitingModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: TransactionAwaitingModalData) {
  }
}
