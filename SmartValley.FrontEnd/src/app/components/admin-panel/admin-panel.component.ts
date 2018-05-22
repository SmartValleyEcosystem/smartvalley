import {Component} from '@angular/core';
import {NgbTabChangeEvent} from '@ng-bootstrap/ng-bootstrap';
import {Location} from '@angular/common';
import {Paths} from '../../paths';
import {MatTabChangeEvent} from '@angular/material';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent {

  public mainTabItems: string[] = ['users', 'projects', 'scorings', 'feedbacks', 'subscriptions'];
  public subTabItems: string[] = ['admins', 'experts', 'applications', 'all'];

  public selectedMainTab = 0;
  public selectedSubTab = this.subTabItems[0];

  constructor(private location: Location) {
  }

  public onMainTabChange($event: MatTabChangeEvent) {
    if ($event.index === 0) {
      this.location.replaceState(Paths.Admin + '/' + this.mainTabItems[$event.index] + '/' + this.subTabItems[0]);
    } else {
      this.location.replaceState(Paths.Admin + '/' + this.mainTabItems[$event.index]);
    }
  }

  public onSubTabChange($event: NgbTabChangeEvent) {
    this.location.replaceState(Paths.Admin + '/users/' + $event.nextId);
  }
}
