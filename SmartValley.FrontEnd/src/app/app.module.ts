import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {BrowserModule} from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {Web3Service} from './services/web3-service';
import {IfAuthenticated} from './routing/IfAuthenticated';
import {AuthenticationService} from './services/authentication-service';
import {HeaderComponent} from './components/header/header.component';
import {MaterialModule} from './shared/material.module';
import {PrimeNgModule} from './shared/prime-ng.module';
import {NotificationService} from './services/notification-service';
import {NotificationsComponent} from './components/common/notifications/notifications.component';
import {BalanceApiClient} from './api/balance/balance-api-client';
import {AuthHeaderInterceptor} from './api/auth-header-interceptor';
import {IfWeb3Initialized} from "./routing/IfWeb3Initialized";

@NgModule({
  declarations: [
    AppComponent,
    AppComponent,
    MetamaskHowtoComponent,
    RootComponent,
    HeaderComponent,
    NotificationsComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    PrimeNgModule,
    AppRoutingModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHeaderInterceptor,
      multi: true
    },
    BalanceApiClient,
    AuthenticationService,
    Web3Service,
    NotificationService,
    IfAuthenticated,
    IfWeb3Initialized],
  bootstrap: [AppComponent]
})
export class AppModule {
}
