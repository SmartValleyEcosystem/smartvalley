import { Component, OnInit } from '@angular/core';
import {DialogService} from '../../services/dialog-service';
import {UserApiClient} from '../../api/user/user-api-client';
import {NotificationsService} from 'angular2-notifications';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.scss']
})
export class FeedbackComponent implements OnInit {
  public isFeedbackHidden = false;
  public isFeedbackSend = 'isFeedbackSend';

  constructor(private dialogService: DialogService,
              private userApiClient: UserApiClient,
              private notificationService: NotificationsService) { }

  ngOnInit() {
    if ( localStorage.getItem(this.isFeedbackSend) ) {
      this.isFeedbackHidden = true;
    }
  }

  public async openFeedbackDialog() {
    const feedBackData = await this.dialogService.showFeedbackDialog();
    if (feedBackData) {
        await this.userApiClient.addFeedbackAsync(feedBackData);
        localStorage.setItem(this.isFeedbackSend, '1');
        this.notificationService.success('Success', 'Feedback is sent');
    }
  }

  public closeFeedback(event) {
    event.stopPropagation();
    this.isFeedbackHidden = true;
  }

}
