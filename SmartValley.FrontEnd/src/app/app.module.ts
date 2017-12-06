import {NgModule} from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AppComponent} from './app.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {BrowserModule} from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {Web3Service} from './services/web3-service';
import {AuthenticationService} from './services/authentication-service';
import {ApplicationApiClient} from './api/application/application-api.client';
import {HeaderComponent} from './components/header/header.component';
import {MaterialModule} from './shared/material.module';
import {BalanceApiClient} from './api/balance/balance-api-client';
import {AuthHeaderInterceptor} from './api/auth-header-interceptor';
import {ApplicationComponent} from './components/application/application.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ProjectManagerContractClient} from './services/project-manager-contract-client';
import {ScoringComponent} from './components/scoring/scoring.component'; // <-- #1 import module
import {MatTabsModule} from '@angular/material';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {FooterComponent} from './components/footer/footer.component';
import {SimpleNotificationsModule} from 'angular2-notifications';
import {EstimateComponent} from './components/estimate/estimate.component';
import {QuestionService} from './services/question-service';
import {ContractApiClient} from './api/contract/contract-api-client';
import {ErrorInterceptor} from './api/error-interceptor';
import {ScoringApiClient} from './api/scoring/scoring-api-client';
import {ProjectCardComponent} from './components/common/project-card/project-card.component';
import {ProjectApiClient} from './api/project/project-api-client';
import {ReportComponent} from './components/report/report.component';
import {EstimatesApiClient} from './api/estimates/estimates-api-client';
import {QuestionsComponent} from './components/report/questions/questions.component';
import {ProjectService} from './services/project-service';
import {NgProgressInterceptor, NgProgressModule} from 'ngx-progressbar';
import {TransactionAwaitingModalComponent} from './components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {BlockiesService} from './services/blockies-service';
import {DialogService} from './services/dialog-service';
import {NullableLinkComponent} from './components/common/nullable-link/nullable-link.component';
import {Ng2DeviceDetectorModule} from 'ng2-device-detector';
import {AlertModalComponent} from './components/common/alert-modal/alert-modal.component';
import {MetamaskManualModalComponent} from './components/common/metamask-manual-modal/metamask-manual-modal.component';
import {GetEtherModalComponent} from './components/common/get-ether-modal/get-ether-modal.component';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {Angulartics2Module} from 'angulartics2';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    RootComponent,
    HeaderComponent,
    ApplicationComponent,
    ScoringComponent,
    FooterComponent,
    EstimateComponent,
    ProjectCardComponent,
    ReportComponent,
    QuestionsComponent,
    TransactionAwaitingModalComponent,
    NullableLinkComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    GetEtherModalComponent
  ],
  entryComponents: [
    TransactionAwaitingModalComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    GetEtherModalComponent
  ],
  imports: [
    MatCheckboxModule,
    MatTabsModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule.forRoot(),
    SimpleNotificationsModule.forRoot(),
    NgProgressModule,
    Ng2DeviceDetectorModule.forRoot(),
    Angulartics2Module.forRoot([Angulartics2GoogleAnalytics]),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NgProgressInterceptor,
      multi: true
    },
    BalanceApiClient,
    ApplicationApiClient,
    ContractApiClient,
    ProjectApiClient,
    ScoringApiClient,
    EstimatesApiClient,
    AuthenticationService,
    ProjectService,
    QuestionService,
    Web3Service,
    DialogService,
    ProjectManagerContractClient,
    BlockiesService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
