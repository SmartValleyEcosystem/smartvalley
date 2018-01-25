import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {VoteModalData} from './vote-modal-data';
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
              private dialogRef: MatDialogRef<VoteModalComponent>) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      amount: [this.data.currentVoteBalance, [
        Validators.max(this.data.currentVoteBalance > 0 ? this.data.currentVoteBalance : this.data.currentBalance),
        Validators.min(0)]
      ],
    });
  }

  submit(form) {
    this.dialogRef.close(form.value.amount);
  }

}

