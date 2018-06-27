import {Component} from '@angular/core';
import {DialogService} from '../../services/dialog-service';
import {NotificationsService} from 'angular2-notifications';
import {FeedbackApiClient} from '../../api/feedback/feedback-api-client';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.scss']
})
export class FeedbackComponent {

  public isFeedbackHidden = false;

  constructor(private dialogService: DialogService,
              private feedbackApiClient: FeedbackApiClient,
              private translateService: TranslateService,
              private notificationService: NotificationsService) {
  }

  public async openFeedbackDialog() {
    const feedBackData = await this.dialogService.showFeedbackDialogAsync();
    if (feedBackData) {
      await this.feedbackApiClient.sendFeedbackAsync(feedBackData);
      this.notificationService.success(
        this.translateService.instant('FeedbackModalComponent.Notify.Success.Title'),
        this.translateService.instant('FeedbackModalComponent.Notify.Success.Text')
      );
    }
  }

  public closeFeedback(event) {
    this.isFeedbackHidden = true;
    event.stopPropagation();
  }
}
