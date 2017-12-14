import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-receipt-modal',
  templateUrl: './receipt-modal.component.html',
  styleUrls: ['./receipt-modal.component.css']
})
export class ReceiptModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  }
}
