import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {MetamaskManualModalData} from './metamask-manual-modal-data';

@Component({
  selector: 'app-metamask-manual-modal',
  templateUrl: './metamask-manual-modal.component.html',
  styleUrls: ['./metamask-manual-modal.component.css']
})
export class MetamaskManualModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: MetamaskManualModalData) {
  }
}
