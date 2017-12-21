import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-receive-ether-modal',
  templateUrl: './receive-ether-modal.component.html',
  styleUrls: ['./receive-ether-modal.component.css']
})
export class ReceiveEtherModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  }
}
