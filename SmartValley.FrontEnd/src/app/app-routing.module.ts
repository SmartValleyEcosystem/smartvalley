import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {RootComponent} from './components/root/root.component';
import {IfAuthenticated, IfWeb3Initialized} from './IfAuthenticated';
import {TryItComponent} from './components/try-it/try-it.component';
import {NgModule} from "@angular/core";
import {MetamaskHowtoComponent} from "./components/metamask-howto/metamask-howto.component";

const appRoutes: Routes = [
  {path: Paths.Root, pathMatch: 'full', component: RootComponent, canActivate: [IfAuthenticated, IfWeb3Initialized]},
  {path: Paths.TryIt, pathMatch: 'full', component: TryItComponent, canActivate: [IfWeb3Initialized]},
  {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
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
