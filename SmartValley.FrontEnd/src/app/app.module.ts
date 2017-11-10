import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule} from '@angular/core';
import {MatButtonModule, MatMenuModule, MatCardModule, MatToolbarModule, MatIconModule} from '@angular/material';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {TestComponent} from './components/test/test.component';
import {TestService} from "./backend/api/test.service";
import 'hammerjs';
import {HttpClient, HttpClientModule} from '@angular/common/http';


@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    TestComponent
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
  providers: [TestService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
