import {Component, OnInit} from '@angular/core';
import {Location} from '@angular/common';
import {Paths} from '../../paths';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  public mainTabItems: string[] = ['users', 'scoring', 'requests'];
  public usersTab: string[] = ['admins', 'experts', 'founders'];
  public scoringTab: string[] = ['public-scoring', 'private-scoring', 'scoring-costs'];
  public requestsTab: string[] = ['requests-for-expert', 'feedback', 'subscriptions'];

  public selectedMainTab = 0;
  public selectedSubTab = 0;

  constructor(private router: Router,
              private route: ActivatedRoute) {}

  public ngOnInit() {
    const mainTab = this.route.snapshot.paramMap.get('mainTab');
    const subTab = this.route.snapshot.paramMap.get('subTab');

    if (mainTab && this.mainTabItems.indexOf(mainTab) !== -1) {
        this.selectedMainTab = this.mainTabItems.indexOf(mainTab);
    }

    if (subTab && this.getSubTabIndex(this.selectedMainTab, subTab) !== -1) {
        this.selectedSubTab = this.getSubTabIndex(this.selectedMainTab, subTab);
    }
  }

  public getSubTabLink(mainTabIndex: number, subItemIndex: number): string {
    switch (mainTabIndex) {
      case 0 :
        return this.usersTab[subItemIndex];
      case 1 :
        return this.scoringTab[subItemIndex];
      case 2 :
        return this.requestsTab[subItemIndex];
    }
  }

  public getSubTabIndex(mainTabIndex: number, subItemName: string): number {
    switch (mainTabIndex) {
      case 0 :
        return this.usersTab.indexOf(subItemName);
      case 1 :
        return this.scoringTab.indexOf(subItemName);
      case 2 :
        return this.requestsTab.indexOf(subItemName);
    }
  }

  public onMainTabChange($event) {
    this.selectedMainTab = $event.index;
    this.selectedSubTab = 0;
    this.router.navigate([Paths.Admin + '/' + this.mainTabItems[$event.index] + '/' + this.getSubTabLink(this.selectedMainTab, this.selectedSubTab)]);
  }

  public onSubTabChange($event, tab: string[]) {
    this.selectedSubTab = $event.index;
    this.router.navigate([Paths.Admin + '/' + this.mainTabItems[this.selectedMainTab] + '/' + tab[$event.index]]);
  }
}
