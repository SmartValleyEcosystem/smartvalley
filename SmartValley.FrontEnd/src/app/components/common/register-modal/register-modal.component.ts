import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {RegisterModalData} from './register-modal-data';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-vote-modal',
  templateUrl: './register-modal.component.html',
  styleUrls: ['./register-modal.component.css']
})
export class RegisterModalComponent implements OnInit {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: RegisterModalData,
              private dialogRef: MatDialogRef<RegisterModalComponent>) {
  }

  async ngOnInit() {
    this.form = this.formBuilder.group({
      email: [this.data.email, [
        Validators.required,
        Validators.email]
      ],
    });
  }

  submit(form) {
    this.dialogRef.close(form.value.email);
  }
}
