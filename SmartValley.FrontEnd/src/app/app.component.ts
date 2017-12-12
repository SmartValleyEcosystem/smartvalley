import {Component, OnInit} from '@angular/core';
import {Options} from 'angular2-notifications';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {QuestionService} from './services/questions/question-service';
import {TranslateService} from '@ngx-translate/core';
import {Subject} from 'rxjs/Subject';
import {Observable} from 'rxjs/Observable';
import {ContractApiClient} from './api/contract/contract-api-client';
import {TokenClient} from './api/token/token-client';

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

  constructor(angulartics2GoogleAnalytics: Angulartics2GoogleAnalytics,
              private questionService: QuestionService,
              private tokenClient: TokenClient,
              translate: TranslateService) {
    translate.use('en');
  }

  async ngOnInit() {
    await this.questionService.initializeAsync();
    await this.tokenClient.loadContractInformationAsync();
  }
}
