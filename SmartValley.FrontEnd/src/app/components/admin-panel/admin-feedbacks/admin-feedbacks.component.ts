import {Component, OnInit} from '@angular/core';
import {LazyLoadEvent} from 'primeng/api';
import {CollectionResponse} from '../../../api/collection-response';
import {FeedbackResponse} from '../../../api/feedback/feedback-response';
import {FeedbackApiClient} from '../../../api/feedback/feedback-api-client';
import {AdminFeedbackItem} from './admin-feedback-item';

@Component({
  selector: 'app-admin-feedbacks',
  templateUrl: './admin-feedbacks.component.html',
  styleUrls: ['./admin-feedbacks.component.css']
})
export class AdminFeedbacksComponent implements OnInit {

  public totalRecords: number;
  public loading: boolean;
  public offset = 0;
  public pageSize = 10;
  public feedbacksResponses: CollectionResponse<FeedbackResponse>;
  public feedbacks: AdminFeedbackItem[] = [];

  constructor(private feedbackApiClient: FeedbackApiClient) {
  }

  public async getFeedbacksList(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadFeedbacksAsync();
  }

  async ngOnInit() {
    await this.loadFeedbacksAsync();
  }

  public renderTableRows(expertResponseItems: FeedbackResponse[]) {
    this.feedbacks = expertResponseItems.map(feedback => <AdminFeedbackItem>{
      firstName: feedback.firstName,
      secondName: feedback.lastName,
      email: feedback.email,
      text: feedback.text
    });
  }

  private async loadFeedbacksAsync(): Promise<void> {
    this.loading = true;
    this.feedbacksResponses = (await this.feedbackApiClient.getFeedbacksListAsync(this.offset, this.pageSize));
    this.totalRecords = this.feedbacksResponses.totalCount;
    this.renderTableRows(this.feedbacksResponses.items);
    this.loading = false;
  }

}
