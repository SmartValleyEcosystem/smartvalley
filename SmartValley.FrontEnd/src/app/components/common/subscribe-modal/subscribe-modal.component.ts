import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {SubscribeRequest} from './subscribe-data';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-subscribe-modal',
  templateUrl: './subscribe-modal.component.html',
  styleUrls: ['./subscribe-modal.component.css']
})
export class SubscribeModalComponent implements OnInit {
  public subscribeForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private subscribeModalComponent: MatDialogRef<SubscribeModalComponent>) { }

  ngOnInit() {
    this.subscribeForm = this.formBuilder.group({
        sum: ['', Validators.required],
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: ['']
    });
  }

  public submit() {
      if (this.subscribeForm.valid) {
          this.subscribeModalComponent.close(
              <SubscribeRequest>this.subscribeForm.value
          );
      }
  }
}
