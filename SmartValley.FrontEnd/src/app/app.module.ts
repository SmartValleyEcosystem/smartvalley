import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {LandingComponent} from './components/landing/landing.component';
import {LoginSuccessComponent} from './components/login-success/login-success.component';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {MatButtonModule, MatCardModule, MatIconModule, MatMenuModule, MatToolbarModule} from '@angular/material';
import {Web3Service} from './services/web3-service';
import {IsAuthorizedGuard} from './is-authorized.guard';
import {LoginInfoService} from './services/login-info-service';

@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    LandingComponent,
    LoginSuccessComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatButtonModule,
    MatMenuModule,
    MatCardModule,
    MatToolbarModule,
    MatIconModule

  ],
  providers: [LoginInfoService, Web3Service, IsAuthorizedGuard],
  bootstrap: [AppComponent]
})
export class AppModule {
}
