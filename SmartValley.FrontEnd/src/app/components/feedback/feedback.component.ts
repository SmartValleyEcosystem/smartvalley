import {Component} from '@angular/core';
import {DialogService} from '../../services/dialog-service';
import {NotificationsService} from 'angular2-notifications';
import {FeedbackApiClient} from '../../api/feedback/feedback-api-client';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.scss']
})
export class FeedbackComponent {

  constructor(private dialogService: DialogService,
              private feedbackApiClient: FeedbackApiClient,
              private notificationService: NotificationsService) {
  }

  public async openFeedbackDialog() {
    const feedBackData = await this.dialogService.showFeedbackDialog();
    if (feedBackData) {
      await this.feedbackApiClient.sendFeedbackAsync(feedBackData);
      this.notificationService.success('Success', 'Feedback is sent');
    }
  }

  public closeFeedback(event) {
    event.stopPropagation();
  }
}
