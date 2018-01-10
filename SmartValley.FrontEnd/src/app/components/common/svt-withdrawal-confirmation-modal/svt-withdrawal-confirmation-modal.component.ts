import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {SvtWithdrawalConfirmationModalData} from './svt-withdrawal-confirmation-modal-data';

@Component({
  selector: 'app-svt-withdrawal-confirmation-modal',
  templateUrl: './svt-withdrawal-confirmation-modal.component.html',
  styleUrls: ['./svt-withdrawal-confirmation-modal.component.css']
})
export class SvtWithdrawalConfirmationModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: SvtWithdrawalConfirmationModalData) {
  }
}
