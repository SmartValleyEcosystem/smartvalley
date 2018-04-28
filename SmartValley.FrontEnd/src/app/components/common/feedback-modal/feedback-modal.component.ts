import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {ChangeStatusModalComponent} from '../change-status-modal/change-status-modal.component';
import {FeedbackData} from './feedback';

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

  constructor(private feedbackModalComponent: MatDialogRef<ChangeStatusModalComponent>) { }

  ngOnInit() {
  }

  public submit() {
      this.feedbackModalComponent.close(
          <FeedbackData>{
          firstName: this.firstName,
          lastName: this.lastName,
          email: this.email,
          text: this.description
        }
      );
  }
}
