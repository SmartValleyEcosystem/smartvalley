import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {LandingComponent} from './components/landing/landing.component';
import {InitializationGuard} from './services/initialization/initialization.guard';
import {InitializationComponent} from './components/initialization/initialization.component';
import {RootComponent} from './components/root/root.component';
import {ShouldHaveEthGuard} from './services/balance/should-have-eth.guard';
import {AccountComponent} from './components/account/account.component';
import {ShouldBeAuthenticatedGuard} from './services/authentication/should-be-authenticated.guard';
import {ShouldBeAdminGuard} from './services/authentication/should-be-admin.guard';
import {AdminPanelComponent} from './components/admin-panel/admin-panel.component';
import {ExpertStatusComponent} from './components/expert-status/expert-status.component';
import {RegisterExpertComponent} from './components/register-expert/register-expert.component';
import {AdminExpertApplicationComponent} from './components/admin-panel/admin-expert-application/admin-expert-application.component';
import {ExpertStatusGuard} from './services/guards/expert-status.guard';
import {ExpertApplicationStatus} from './services/expert/expert-application-status.enum';
import {OfferStatusGuard} from './services/guards/offer-status.guard';
import {ProjectListComponent} from './components/project-list/project-list.component';
import {RegisterComponent} from './components/authentication/register/register.component';
import {RegisterConfirmComponent} from './components/authentication/register-confirm/register-confirm.component';
import {ConfirmEmailComponent} from './components/common/confirm-email/confirm-email.component';
import {CreateProjectComponent} from './components/create-project/create-project.component';
import {EditScoringApplicationComponent} from './components/edit-scoring-application/edit-scoring-application.component';
import {ProjectComponent} from './components/project/project.component';
import {OfferDetailsComponent} from './components/scoring/offer-details/offer-details.component';
import {SubmittedScoringApplicationGuard} from './services/guards/submitted-scoring-application.guard';
import {OfferStatus} from './api/scoring-offer/offer-status.enum';

const appRoutes: Routes = [
  {path: Paths.Initialization, component: InitializationComponent},
  {
    path: Paths.Root, component: RootComponent, canActivate: [InitializationGuard], children: [
    {path: Paths.Root, pathMatch: 'full', component: LandingComponent},
    {
      path: Paths.Admin,
      pathMatch: 'full',
      component: AdminPanelComponent,
      canActivate: [ShouldBeAdminGuard]
    },
    {path: Paths.AdminExpertApplication + '/:id', pathMatch: 'full', component: AdminExpertApplicationComponent},
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
    {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
    {
      path: Paths.ScoringOffer + '/:id/:areaType',
      component: OfferDetailsComponent,
      canActivate: [OfferStatusGuard],
      data: {
        offerStatuses: [
          OfferStatus.Pending
        ]
      }
    },
    {path: Paths.Account, pathMatch: 'full', component: AccountComponent, canActivate: [ShouldBeAuthenticatedGuard]},
    {
      path: Paths.Project,
      pathMatch: 'full',
      component: CreateProjectComponent
    },
    {path: Paths.ProjectEdit, component: CreateProjectComponent},
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
      canActivate: [ExpertStatusGuard, ShouldHaveEthGuard],
      component: RegisterExpertComponent,
      data: {
        expertStatuses: [
          ExpertApplicationStatus.None,
          ExpertApplicationStatus.Rejected
        ]
      }
    },
    {path: Paths.ProjectList, component: ProjectListComponent},
    {path: Paths.ProjectList + '/:search', component: ProjectListComponent},
    {path: Paths.MyProject + '/:id', component: ProjectComponent},
    {
      path: Paths.ScoringApplication + '/:id',
      component: EditScoringApplicationComponent,
      canActivate: [SubmittedScoringApplicationGuard]
    }
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
