import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {InvestRequest} from './invest-data';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-invest-modal',
  templateUrl: './invest-modal.component.html',
  styleUrls: ['./invest-modal.component.css']
})
export class InvestModalComponent implements OnInit {
  public investForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private investModalComponent: MatDialogRef<InvestModalComponent>) { }

  ngOnInit() {
    this.investForm = this.formBuilder.group({
        sum: ['', Validators.required],
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: ['']
    });
  }

  public submit() {
      if (this.investForm.valid) {
          this.investModalComponent.close(
              <InvestRequest>this.investForm.value
          );
      }
  }
}
