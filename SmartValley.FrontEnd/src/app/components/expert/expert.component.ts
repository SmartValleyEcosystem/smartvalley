import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import {Paths} from '../../paths';

@Component({
  selector: 'app-expert',
  templateUrl: './expert.component.html',
  styleUrls: ['./expert.component.css']
})
export class ExpertComponent {
    public tabItems: string[] = ['WorkPlace', 'Offers', 'OffersHistory'];
    public tabFromUrlParam = this.route.snapshot.paramMap.get('tab');
    public selectedTab = this.tabItems.indexOf(this.tabFromUrlParam);

    constructor(private route: ActivatedRoute,
                private router: Router) {
    }

    ngOnInit() {
        if (!this.tabFromUrlParam) {
            this.tabFromUrlParam = this.tabItems[0];
        };
    }

    public changeTab(event) {
      this.router.navigate([ Paths.Expert, { tab: this.tabItems[event.index] }]);
    }
}
