import {Component, Inject, OnInit} from '@angular/core';
import {ReceiveTokensModalData} from './receive-tokens-modal-data';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-receive-tokens-modal',
  templateUrl: './receive-tokens-modal.component.html',
  styleUrls: ['./receive-tokens-modal.component.scss']
})
export class ReceiveTokensModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: ReceiveTokensModalData,
              private modal: MatDialogRef<ReceiveTokensModalComponent>) {
  }

  ngOnInit() {
  }

  public submit() {
    this.modal.close(true);
  }
}
