import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {SetFreezeTimeModalData} from './set-freeze-time-modal-data';

@Component({
  selector: 'app-set-freeze-time-modal',
  templateUrl: './set-freeze-time-modal.component.html',
  styleUrls: ['./set-freeze-time-modal.component.scss']
})
export class SetFreezeTimeModalComponent implements OnInit {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: SetFreezeTimeModalData,
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
