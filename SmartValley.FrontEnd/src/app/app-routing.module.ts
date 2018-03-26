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
import {ShouldBeAdminGuard} from './services/authentication/should-be-admin.guard';
import {MyProjectsComponent} from './components/my-projects/my-projects.component';
import {VotingComponent} from './components/voting/voting.component';
import {VotingCardComponent} from './components/voting-card/voting-card.component';
import {CompletedVotingComponent} from './components/completed-voting/completed-voting.component';
import {AdminPanelComponent} from './components/admin-panel/admin-panel.component';
import {CompletedVotingsComponent} from './components/completed-votings/completed-votings.component';
import {ExpertStatusComponent} from './components/expert-status/expert-status.component';
import {ExpertComponent} from './components/expert/expert.component';
import {RegisterExpertComponent} from './components/register-expert/register-expert.component';
import {AdminExpertApplicationComponent} from './components/admin-panel/admin-expert-application/admin-expert-application.component';
import {ExpertStatusGuard} from './services/guards/expert-status.guard';
import {ExpertApplicationStatus} from './services/expert/expert-application-status.enum';
import {ExpertShouldBeAssignedGuard} from './services/guards/expert-should-be-assigned.guard';
import {ProjectListComponent} from './components/project-list/project-list.component';
import {RegisterComponent} from './components/authentication/register/register.component';
import {RegisterConfirmComponent} from './components/authentication/register-confirm/register-confirm.component';
import {ConfirmEmailComponent} from './components/common/confirm-email/confirm-email.component';

const appRoutes: Routes = [
  {path: Paths.Initialization, component: InitializationComponent},
  {
    path: Paths.Root, component: RootComponent, canActivate: [InitializationGuard], children: [
      {path: Paths.Root, pathMatch: 'full', component: LandingComponent},
      {path: Paths.Voting, pathMatch: 'full', component: VotingComponent},
      {
        path: Paths.MyProjects,
        pathMatch: 'full',
        component: MyProjectsComponent,
        canActivate: [ShouldBeAuthenticatedGuard]
      },
      {
        path: Paths.Admin,
        pathMatch: 'full',
        component: AdminPanelComponent,
        canActivate: [ShouldBeAdminGuard]
      },
      {path: Paths.AdminExpertApplication + '/:id', pathMatch: 'full', component: AdminExpertApplicationComponent},
      {
        path: Paths.Voting + '/:id',
        pathMatch: 'full',
        component: VotingCardComponent,
        canActivate: [ShouldBeAdminGuard]
      },
      {
        path: Paths.CompletedVoting + '/:address',
        pathMatch: 'full',
        component: CompletedVotingComponent
      },
      {
        path: Paths.Register,
        pathMatch: 'full',
        component: RegisterComponent
      },
      {path: Paths.ConfirmEmail, component: ConfirmEmailComponent},
      {
        path: Paths.ConfirmRegister,
        pathMatch: 'full',
        component: RegisterConfirmComponent
      },
      {
        path: Paths.CompletedVotings,
        pathMatch: 'full',
        component: CompletedVotingsComponent
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
          guards: [GuardType.ShouldHaveEth]
        }
      },
      {path: Paths.Report + '/:id', pathMatch: 'full', component: ReportComponent},
      {
        path: Paths.ExpertStatus,
        canActivate: [ExpertStatusGuard],
        component: ExpertStatusComponent,
        data: {
          expertStatuses: [
            ExpertApplicationStatus.None,
            ExpertApplicationStatus.Rejected,
            ExpertApplicationStatus.Pending
          ]
        }
      },
      {
        path: Paths.RegisterExpert,
        canActivate: [ExpertStatusGuard],
        component: RegisterExpertComponent,
        data: {
          expertStatuses: [
            ExpertApplicationStatus.None,
            ExpertApplicationStatus.Rejected
          ]
        }
      },
      {
        path: Paths.Expert + '/:tab',
        component: ExpertComponent,
        canActivate: [ExpertStatusGuard],
        data: {
          expertStatuses: [ExpertApplicationStatus.Accepted]
        }
      },
      {
        path: Paths.Expert,
        component: ExpertComponent,
        canActivate: [ExpertStatusGuard],
        data: {
          expertStatuses: [ExpertApplicationStatus.Accepted]
        }
      },
      {
        path: Paths.Scoring + '/:id',
        pathMatch: 'full',
        component: EstimateComponent,
        canActivate: [ShouldHaveEthGuard, ExpertShouldBeAssignedGuard]
      },
      {path: Paths.ProjectList, component: ProjectListComponent},
      {path: Paths.ProjectList + '/:search', component: ProjectListComponent}
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
