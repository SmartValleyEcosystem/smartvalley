import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {VoteModalData} from './vote-modal-data';
import {TokenContractClient} from '../../../services/contract-clients/token-contract-client';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-vote-modal',
  templateUrl: './vote-modal.component.html',
  styleUrls: ['./vote-modal.component.css']
})
export class VoteModalComponent implements OnInit {

  private form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: VoteModalData,
              private dialogRef: MatDialogRef<VoteModalComponent>,
              private tokenService: TokenContractClient) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      amount: [this.data.currentVoteBalance, [
        Validators.max(this.data.currentVoteBalance > 0 ? this.data.currentVoteBalance : this.data.currentBalance),
        Validators.pattern('^\\s*(?=.*[1-9])\\d*(?:\\.\\d{1,' + this.tokenService.decimals + '})?\\s*$')]
      ],
    });
  }

  submit(form) {
    this.dialogRef.close(form.value.amount);
  }

}


