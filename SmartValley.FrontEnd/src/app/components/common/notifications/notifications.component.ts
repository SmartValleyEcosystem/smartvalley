import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs/Subscription';
import {Message} from 'primeng/primeng';
import {NotificationService} from '../../../services/notification-service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit, OnDestroy {
  msgs: Message[] = [];
  subscription: Subscription;

  constructor(private notificationsService: NotificationService) { }

  ngOnInit() {
    this.subscribeToNotifications();
  }

  subscribeToNotifications() {
    this.subscription = this.notificationsService.notificationChange
      .subscribe(notification => {
        this.msgs.length = 0;
        this.msgs.push(notification);
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
