import {Component, OnInit} from '@angular/core';
import {LazyLoadEvent} from 'primeng/api';
import {CollectionResponse} from '../../../api/collection-response';
import {AdminSubscriberItem} from './admin-subscription-item';
import {SubscriptionApiClient} from '../../../api/subscription/subscription-api-client';
import {SubscriptionResponse} from '../../../api/subscription/subscription-response';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';

@Component({
  selector: 'app-admin-subscriptions',
  templateUrl: './admin-subscriptions.component.html',
  styleUrls: ['./admin-subscriptions.component.scss']
})
export class AdminSubscriptionsComponent implements OnInit {

  public totalRecords: number;
  public loading = true;
  public offset = 0;
  public pageSize = 10;
  public subscribersResponses: CollectionResponse<SubscriptionResponse>;
  public subscribers: AdminSubscriberItem[] = [];

  constructor(private router: Router,
              private subscriberApiClient: SubscriptionApiClient) {
  }

  public async getSubscriptionsList(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadSubscriptionsResponsesAsync();
  }

  async ngOnInit() {
    await this.loadSubscriptionsResponsesAsync();
  }

  public renderTableRows(subscriberResponse: SubscriptionResponse[]) {
    this.subscribers = subscriberResponse.map(subscriber => <AdminSubscriberItem>{
      name: subscriber.name,
      phone: subscriber.phone,
      email: subscriber.email,
      sum: subscriber.sum,
      projectId: subscriber.projectId,
      projectName: subscriber.projectName
    });
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  private async loadSubscriptionsResponsesAsync(): Promise<void> {
    this.subscribersResponses = await this.subscriberApiClient.getAsync(this.offset, this.pageSize);
    this.totalRecords = this.subscribersResponses.totalCount;
    this.renderTableRows(this.subscribersResponses.items);
    this.loading = false;
  }

}
