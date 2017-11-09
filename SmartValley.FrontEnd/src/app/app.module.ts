import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule} from '@angular/core';
import {HttpModule} from '@angular/http';
import {MatButtonModule, MatMenuModule, MatCardModule, MatToolbarModule, MatIconModule} from '@angular/material';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {TestService} from './backend/api/test.service';
import 'hammerjs';
import {LandingComponent} from './components/landing/landing.component';
import {LoginSuccessComponent} from './components/login-success/login-success.component';
import {Web3Service} from './services/web3-service';

@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    LandingComponent,
    LoginSuccessComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatButtonModule,
    MatMenuModule,
    MatCardModule,
    MatToolbarModule,
    MatIconModule

  ],
  providers: [TestService, Web3Service],
  bootstrap: [AppComponent]
})
export class AppModule {
}
