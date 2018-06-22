import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ReturnAddressModalData} from './return-address-modal-data';

@Component({
  selector: 'app-return-address-dialog',
  templateUrl: './return-address-modal.component.html',
  styleUrls: ['./return-address-modal.component.scss']
})
export class ReturnAddressModalComponent implements OnInit {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: ReturnAddressModalData,
              private modal: MatDialogRef<ReturnAddressModalComponent>) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      returnAddress: [this.data.returnAddress, [Validators.required]]
    });
  }

  public submit() {
    if (this.form.valid) {
      this.modal.close(this.form.value.returnAddress);
    }
  }
}
