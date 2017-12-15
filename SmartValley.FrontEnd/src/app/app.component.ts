import {Component, OnInit} from '@angular/core';
import {Options} from 'angular2-notifications';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {QuestionService} from './services/questions/question-service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  notifyOptions: Options = {
    position: ['top', 'left'],
    timeOut: 3000,
    animate: 'fromLeft',
    showProgressBar: false
  };

  constructor(angulartics2GoogleAnalytics: Angulartics2GoogleAnalytics,
              translate: TranslateService) {
    translate.use('en');
  }
}
