import {Component} from '@angular/core';
import {Options} from 'angular2-notifications';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {TranslateService} from '@ngx-translate/core';
import {Angulartics2} from 'angulartics2';
import {environment} from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  notifyOptions: Options = {
    position: ['top', 'left'],
    timeOut: 3000,
    theClass: 'notification',
    animate: 'fromLeft',
    showProgressBar: false
  };

  constructor(angulartics2GoogleAnalytics: Angulartics2GoogleAnalytics,
              angulartics2: Angulartics2,
              translate: TranslateService) {
    if (!environment.production) {
      angulartics2.developerMode(true);
    }
    translate.use('en');
  }
}
