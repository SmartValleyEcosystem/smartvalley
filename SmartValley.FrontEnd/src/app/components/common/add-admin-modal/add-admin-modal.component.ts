import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators, AbstractControl} from '@angular/forms';

@Component({
  selector: 'app-add-admin-modal',
  templateUrl: './add-admin-modal.component.html',
  styleUrls: ['./add-admin-modal.component.css']
})
export class AddAdminModalComponent implements OnInit {
  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private dialogRef: MatDialogRef<AddAdminModalComponent>) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      address: ['', [Validators.required, AddAdminModalComponent.validateWalletAddress]],
    });
  }

  submit(form) {
    this.dialogRef.close(form.value.address);
  }

  public static validateWalletAddress(control: AbstractControl) {
    const address = control.value;
    const error = {'walletAddress': true}
    if (!address || /^(0x)[0-9a-f]{40}$/i.test(address)) {
      return null;
    }
    return error;
  }
}
