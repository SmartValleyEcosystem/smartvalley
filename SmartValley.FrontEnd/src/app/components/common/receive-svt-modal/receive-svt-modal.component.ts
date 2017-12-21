import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-receive-svt-modal',
  templateUrl: './receive-svt-modal.component.html',
  styleUrls: ['./receive-svt-modal.component.css']
})
export class ReceiveSvtModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  }

}
