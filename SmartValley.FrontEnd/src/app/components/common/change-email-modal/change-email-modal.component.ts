import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-change-email-modal',
  templateUrl: './change-email-modal.component.html',
  styleUrls: ['./change-email-modal.component.css']
})
export class ChangeEmailModalComponent implements OnInit {
  public form: FormGroup;
  public email: string;

  constructor(private formBuilder: FormBuilder,
              private dialogRef: MatDialogRef<ChangeEmailModalComponent>) {
  }

  async ngOnInit() {
    this.form = this.formBuilder.group({
      email: [this.email, [
        Validators.required,
        Validators.email]
      ],
    });
  }

  submit(form) {
    this.dialogRef.close(form.value.email);
  }
}
