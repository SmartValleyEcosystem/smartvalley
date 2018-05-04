import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {FeedbackRequest} from './feedback';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-feedback-modal',
  templateUrl: './feedback-modal.component.html',
  styleUrls: ['./feedback-modal.component.css']
})
export class FeedbackModalComponent implements OnInit {
  public firstName: string;
  public lastName: string;
  public email: string;
  public description: string;
  public feedbackForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private feedbackModalComponent: MatDialogRef<FeedbackModalComponent>) { }

  ngOnInit() {
    this.feedbackForm = this.formBuilder.group({
      firstName: ['', Validators.maxLength(50)],
      secondName: ['', Validators.maxLength(50)],
      email: ['', [Validators.maxLength(50), Validators.pattern('^$|\\w+@\\w+\\.\\w+')]],
      description: ['', [Validators.required, Validators.maxLength(1500)]]
    });
  }

  public submit() {
      this.feedbackModalComponent.close(
          <FeedbackRequest>{
          firstName: this.feedbackForm.value.firstName,
          lastName: this.feedbackForm.value.lastName,
          email: this.feedbackForm.value.email === '' ? null : this.feedbackForm.value.email,
          text: this.feedbackForm.value.description
        }
      );
  }
}
