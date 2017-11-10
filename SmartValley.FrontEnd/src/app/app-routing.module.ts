import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LandingComponent} from './components/landing/landing.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {LoginSuccessComponent} from './components/login-success/login-success.component';
import {IsAuthorizedGuard} from './is-authorized.guard';
import {Paths} from './paths';

const appRoutes: Routes = [
  { path: '', redirectTo: Paths.Landing,  pathMatch: 'full' },
  { path: Paths.Landing,  pathMatch: 'full', component: LandingComponent },
  { path: Paths.MetaMaskHowTo,  pathMatch: 'full', component: MetamaskHowtoComponent },
  { path: Paths.LoggedIn,  pathMatch: 'full', component: LoginSuccessComponent, canActivate: [IsAuthorizedGuard] },
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
