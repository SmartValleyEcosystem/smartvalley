import {NgModule} from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AppComponent} from './app.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {BrowserModule} from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
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
import {ReactiveFormsModule} from '@angular/forms';
import {ProjectManagerContractClient} from './services/project-manager-contract-client';
import {ScoringComponent} from './components/scoring/scoring.component';  // <-- #1 import module
import {MatTabsModule} from '@angular/material';
import {FooterComponent} from './components/footer/footer.component';
import {SimpleNotificationsModule} from 'angular2-notifications';
import {EstimateComponent} from './components/estimate/estimate.component';
import {ScoringService} from './services/scoring-service';
import {QuestionService} from './services/question-service';
import {ApplicationService} from './services/application-service';
import {ContractApiClient} from './api/contract/contract-api-client';
import {ErrorInterceptor} from './api/error-interceptor';
import {ProjectApiClient} from './api/project/project-api-client';

@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    RootComponent,
    HeaderComponent,
    ApplicationComponent,
    ScoringComponent,
    FooterComponent,
    EstimateComponent
  ],
  imports: [
    MatTabsModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgbModule.forRoot(),
    SimpleNotificationsModule.forRoot()
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
    BalanceApiClient,
    ApplicationApiClient,
    ContractApiClient,
    ProjectApiClient,
    AuthenticationService,
    ApplicationService,
    QuestionService,
    ScoringService,
    Web3Service,
    ProjectManagerContractClient],
  bootstrap: [AppComponent]
})
export class AppModule {
}
