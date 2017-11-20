import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {ApplicationComponent} from './components/application/application.component';
import {ScoringComponent} from './components/scoring/scoring.component';


const appRoutes: Routes = [
  {path: '', pathMatch: 'full', component: RootComponent},
  {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
  {path: Paths.Application, pathMatch: 'full', component: ApplicationComponent},
  {path: Paths.Scoring, pathMatch: 'full', component: ScoringComponent}
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
