import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {IfWeb3Initialized} from './routing/IfWeb3Initialized';
import {ApplicationComponent} from "./components/application/application.component";
import {ScorringComponent} from "./components/scorring/scorring.component";

const appRoutes: Routes = [
  {path: Paths.Root, pathMatch: 'full', component: RootComponent, canActivate: [IfWeb3Initialized]},
  {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
  {path: Paths.BecomeExpert, pathMatch: 'full', component: ScorringComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
