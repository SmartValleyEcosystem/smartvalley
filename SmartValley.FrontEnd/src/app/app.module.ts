import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule} from '@angular/core';
import {HttpModule} from "@angular/http";
import {MatButtonModule, MatMenuModule, MatCardModule, MatToolbarModule, MatIconModule} from '@angular/material';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {TestComponent} from './components/test/test.component';
import {TestService} from "./backend/api/test.service";
import 'hammerjs';


@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    TestComponent
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
  providers: [TestService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
