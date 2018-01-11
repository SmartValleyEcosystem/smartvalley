import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {LandingComponent} from './components/landing/landing.component';
import {ApplicationComponent} from './components/application/application.component';
import {ScoringComponent} from './components/scoring/scoring.component';
import {EstimateComponent} from './components/estimate/estimate.component';
import {ReportComponent} from './components/report/report.component';
import {InitializationGuard} from './services/initialization/initialization.guard';
import {InitializationComponent} from './components/initialization/initialization.component';
import {RootComponent} from './components/root/root.component';
import {ShouldHaveEthGuard} from './services/balance/should-have-eth.guard';
import {AccountComponent} from './components/account/account.component';
import {CompositeGuard} from './services/guards/composite.guard';
import {GuardType} from './services/guards/guard-type.enum';
import {ShouldBeAuthenticatedGuard} from './services/authentication/should-be-authenticated.guard';
import {MyProjectsComponent} from './components/my-projects/my-projects.component';
import {VotingComponent} from './components/voting/voting.component';

const appRoutes: Routes = [
  {path: Paths.Initialization, component: InitializationComponent},
  {
    path: Paths.Root, component: RootComponent, canActivate: [InitializationGuard], children: [
      {path: Paths.Root, pathMatch: 'full', component: LandingComponent},
      {path: Paths.Voting, pathMatch: 'full', component: VotingComponent, canActivate: [ShouldBeAuthenticatedGuard]},
      {
        path: Paths.MyProjects,
        pathMatch: 'full',
        component: MyProjectsComponent,
        canActivate: [ShouldBeAuthenticatedGuard]
      },
      {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
      {path: Paths.Scoring, pathMatch: 'full', component: ScoringComponent},
      {path: Paths.Account, pathMatch: 'full', component: AccountComponent, canActivate: [ShouldBeAuthenticatedGuard]},
      {
        path: Paths.Application,
        pathMatch: 'full',
        component: ApplicationComponent,
        canActivate: [CompositeGuard],
        data: {
          guards: [
            GuardType.ShouldHaveEth,
            GuardType.ShouldHaveSvt
          ]
        }
      },
      {path: Paths.Report + '/:id', pathMatch: 'full', component: ReportComponent},
      {path: Paths.Scoring + '/:id', pathMatch: 'full', component: EstimateComponent, canActivate: [ShouldHaveEthGuard]}
    ]
  },
  {path: '**', redirectTo: Paths.Root}
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
