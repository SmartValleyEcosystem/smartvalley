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
  public requestsTab: string[] = ['requests-for-expert', 'feedback', 'invest'];

  public selectedMainTab = 0;
  public selectedSubTab = 0;

  constructor(
        private location: Location,
        private router: Router,
        private route: ActivatedRoute) {
      this.route.params.subscribe( params => this.selectedMainTab = this.mainTabItems.indexOf(params['mainTab']) );
  }

  public ngOnInit() {
    this.selectedMainTab = this.mainTabItems.indexOf(this.route.snapshot.paramMap.get('mainTab'));
    if (this.route.snapshot.paramMap.get('subTab')) {
        this.selectedSubTab = this.getSubTabIndex(this.selectedMainTab, this.route.snapshot.paramMap.get('subTab'));
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
    this.selectedSubTab =  0;
    this.router.navigate([Paths.Admin + '/' + this.mainTabItems[$event.index] + '/']);
  }

  public onSubTabChange($event, tab: string[]) {
    this.router.navigate([Paths.Admin + '/' + this.mainTabItems[this.selectedMainTab] + '/' + tab[$event.index]]);
  }
}
