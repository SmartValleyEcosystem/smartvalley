import {Component, OnInit} from '@angular/core';
import {Options} from 'angular2-notifications';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {QuestionService} from './services/question-service';
import {TranslateService} from '@ngx-translate/core';
import {Subject} from 'rxjs/Subject';
import {Observable} from 'rxjs/Observable';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  notifyOptions: Options = {
    position: ['top', 'left'],
    timeOut: 3000,
    animate: 'fromLeft',
    showProgressBar: false
  };

  lang = new Subject<string>();
  lang$: Observable<string>;

  constructor(angulartics2GoogleAnalytics: Angulartics2GoogleAnalytics,
              private questionService: QuestionService,
              translate: TranslateService) {
    // the lang to use, if the lang isn't available, it will use the current loader to get them
    translate.use('ru');
  }

  async ngOnInit() {
    await this.questionService.initializeQestionsCollectionAsync();
  }
}
