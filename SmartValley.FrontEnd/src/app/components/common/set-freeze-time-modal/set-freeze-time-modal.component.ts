import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-set-freeze-time-modal',
  templateUrl: './set-freeze-time-modal.component.html',
  styleUrls: ['./set-freeze-time-modal.component.scss']
})
export class SetFreezeTimeModalComponent implements OnInit {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data,
              private modal: MatDialogRef<SetFreezeTimeModalComponent>) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      freezeTime: [this.data.freezeTime, [Validators.required]]
    });
  }

  public submit() {
    if (this.form.valid) {
      this.modal.close(this.form.value.freezeTime);
    }
  }
}
