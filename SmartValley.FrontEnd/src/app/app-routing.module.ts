import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './components/landing/landing.component';
import { MetamaskHowtoComponent } from './components/metamask-howto/metamask-howto.component';
import { LoginSuccessComponent } from './components/login-success/login-success.component';
import { IsAuthorizedGuard } from './is-authorized.guard';

const appRoutes: Routes = [
  { path: '', redirectTo: '/landing',  pathMatch: 'full' },
  { path: 'landing',  pathMatch: 'full', component: LandingComponent },
  { path: 'metamaskhowto',  pathMatch: 'full', component: MetamaskHowtoComponent },
  { path: 'loggedin',  pathMatch: 'full', component: LoginSuccessComponent, canActivate: [IsAuthorizedGuard] },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
