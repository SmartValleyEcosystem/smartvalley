import {NgModule} from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AppComponent} from './app.component';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {LandingComponent} from './components/landing/landing.component';
import {BrowserModule} from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {Web3Service} from './services/web3-service';
import {AuthenticationService} from './services/authentication/authentication-service';
import {HeaderComponent} from './components/header/header.component';
import {MaterialModule} from './shared/material.module';
import {BalanceApiClient} from './api/balance/balance-api-client';
import {ExpertApiClient} from './api/expert/expert-api-client';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ScoringManagerContractClient} from './services/contract-clients/scoring-manager-contract-client';
import {CheckboxModule} from 'primeng/checkbox';
import {MatTabsModule} from '@angular/material';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSelectModule} from '@angular/material/select';
import {FooterComponent} from './components/footer/footer.component';
import {SimpleNotificationsModule} from 'angular2-notifications';
import {ScoringCriterionService} from './services/criteria/scoring-criterion.service';
import {ContractApiClient} from './api/contract/contract-api-client';
import {ErrorInterceptor} from './api/error-interceptor';
import {AdminContractClient} from './services/contract-clients/admin-contract-client';
import {ScoringApiClient} from './api/scoring/scoring-api-client';
import {ProjectInfoComponent} from './components/project-info/project-info.component';
import {ProjectApiClient} from './api/project/project-api-client';
import {EstimatesApiClient} from './api/estimates/estimates-api-client';
import {UserApiClient} from './api/user/user-api-client';
import {ScoreColorsService} from './services/project/score-colors.service';
import {DictionariesService} from './services/common/dictionaries.service';
import {NgProgressInterceptor, NgProgressModule} from 'ngx-progressbar';
import {TransactionAwaitingModalComponent} from './components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {BlockiesService} from './services/blockies-service';
import {DialogService} from './services/dialog-service';
import {NullableLinkComponent} from './components/common/nullable-link/nullable-link.component';
import {Ng2DeviceDetectorModule} from 'ng2-device-detector';
import {AlertModalComponent} from './components/common/alert-modal/alert-modal.component';
import {MetamaskManualModalComponent} from './components/common/metamask-manual-modal/metamask-manual-modal.component';
import {Angulartics2GoogleAnalytics} from 'angulartics2/ga';
import {Angulartics2Module} from 'angulartics2';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {multiTranslateLoaderFactory} from './services/translate/multi-translate-loader-factory';
import {BalanceService} from './services/balance/balance.service';
import {InitializationComponent} from './components/initialization/initialization.component';
import {InitializationService} from './services/initialization/initialization.service';
import {InitializationGuard} from './services/initialization/initialization.guard';
import {RootComponent} from './components/root/root.component';
import {ReceiveEtherModalComponent} from './components/common/receive-ether-modal/receive-ether-modal.component';
import {ShouldHaveEthGuard} from './services/balance/should-have-eth.guard';
import {AccountComponent} from './components/account/account.component';
import {MatIconModule} from '@angular/material/icon';
import {CompositeGuard} from './services/guards/composite.guard';
import {GuardFactory} from './services/guards/guard-factory';
import {DashIfEmptyPipe} from './utils/dash-if-empty.pipe';
import {ShouldBeAuthenticatedGuard} from './services/authentication/should-be-authenticated.guard';
import {ScoringExpertsManagerContractClient} from './services/contract-clients/scoring-experts-manager-contract-client';
import {FormatDatePipe} from './utils/format-date.pipe';
import {AuthenticationApiClient} from './api/authentication/authentication-api-client';
import {UserContext} from './services/authentication/user-context';
import {JwtInterceptor} from './api/jwt-interceptor';
import {AdminApiClient} from './api/admin/admin-api-client';
import {AdminPanelComponent} from './components/admin-panel/admin-panel.component';
import {AddAdminModalComponent} from './components/common/add-admin-modal/add-admin-modal.component';
import {ShouldBeAdminGuard} from './services/authentication/should-be-admin.guard';
import {FileUploadModule} from 'primeng/fileupload';
import {TableModule} from 'primeng/table';
import {DataTableModule} from 'primeng/primeng';
import {DropdownModule} from 'primeng/dropdown';
import {ExpertsRegistryContractClient} from './services/contract-clients/experts-registry-contract-client';
import {CalendarModule} from 'primeng/calendar';
import {ExpertStatusComponent} from './components/expert-status/expert-status.component';
import {RegisterExpertComponent} from './components/register-expert/register-expert.component';
import {AdminExpertApplicationsListComponent} from './components/admin-panel/admin-expert-applications-list/admin-expert-applications-list.component';
import {AdminExpertApplicationComponent} from './components/admin-panel/admin-expert-application/admin-expert-application.component';
import {AreaService} from './services/expert/area.service';
import {EnumHelper} from './utils/enum-helper';
import {AdminExpertsListComponent} from './components/admin-panel/admin-experts-list/admin-experts-list.component';
import {CreateNewExpertModalComponent} from './components/common/create-new-expert-modal/create-new-expert-modal.component';
import {EditExpertModalComponent} from './components/common/edit-expert-modal/edit-expert-modal.component';
import {ExpertStatusGuard} from './services/guards/expert-status.guard';
import {AdminScoringProjectsComponent} from './components/admin-panel/admin-scoring-projects/admin-scoring-projects.component';
import {SetExpertsModalComponent} from './components/common/set-experts-modal/set-experts-modal.component';
import {ChangeEmailModalComponent} from './components/common/change-email-modal/change-email-modal.component';
import {OffersApiClient} from './api/scoring-offer/offers-api-client';
import {RoundNumberPipe} from './utils/round-number.pipe';
import {SearchWithAutocompleteComponent} from './components/search-with-autocomplete/search-with-autocomplete.component';
import {ProjectListComponent} from './components/project-list/project-list.component';
import {PaginatorModule} from 'primeng/paginator';
import {TooltipModule} from 'primeng/tooltip';
import {RadioButtonModule} from 'primeng/radiobutton';
import {OfferStatusGuard} from './services/guards/offer-status.guard';
import {SelectComponent} from './components/select/select.component';
import {AutocompleteComponent} from './components/autocomplete/autocomplete.component';
import {EditScoringApplicationComponent} from './components/edit-scoring-application/edit-scoring-application.component';
import {StickyModule} from 'ng2-sticky-kit';
import {InputSwitchComponent} from './components/input-switch/input-switch.component';
import {ScoringService} from './services/scoring/scoring.service';
import {ScoringContractClient} from './services/contract-clients/scoring-contract-client';
import {ScoringCostComponent} from './components/common/scoring-cost-modal/scoring-cost.component';
import {ConfirmEmailComponent} from './components/common/confirm-email/confirm-email.component';
import {RegisterConfirmComponent} from './components/authentication/register-confirm/register-confirm.component';
import {RegisterComponent} from './components/authentication/register/register.component';
import {CreateProjectComponent} from './components/create-project/create-project.component';
import {WelcomeModalComponent} from './components/common/welcome-modal/welcome-modal.component';
import {ScoringApplicationApiClient} from './api/scoring-application/scoring-application-api-client';
import {DeleteProjectModalComponent} from './components/common/delete-project-modal/delete-project-modal.component';
import {ProjectComponent} from './components/project/project.component';
import {ProjectAboutComponent} from './components/project/project-about/project-about.component';
import {WaitingModalComponent} from './components/common/waiting-modal/waiting-modal.component';
import {ScoringApplicationComponent} from './components/project/scoring-application/scoring-application.component';
import {ProjectService} from './services/project/project.service';
import {ExpertScoringComponent} from './components/expert-scoring/expert-scoring.component';
import {ScoringPaymentComponent} from './components/scoring/scoring-payment/scoring-payment.component';
import {ExpertSelectorComponent} from './components/scoring/scoring-payment/expert-selector/expert-selector.component';
import {OfferDetailsComponent} from './components/scoring/offer-details/offer-details.component';
import {SubmittedScoringApplicationGuard} from './services/guards/submitted-scoring-application.guard';
import {ScoringListComponent} from './components/scoring-list/scoring-list.component';

@NgModule({
  declarations: [
    AppComponent,
    MetamaskHowtoComponent,
    LandingComponent,
    HeaderComponent,
    CreateProjectComponent,
    ScoringListComponent,
    FooterComponent,
    ProjectInfoComponent,
    TransactionAwaitingModalComponent,
    ScoringCostComponent,
    NullableLinkComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    InitializationComponent,
    RootComponent,
    ReceiveEtherModalComponent,
    AccountComponent,
    DashIfEmptyPipe,
    EditScoringApplicationComponent,
    FormatDatePipe,
    AdminPanelComponent,
    AddAdminModalComponent,
    ExpertStatusComponent,
    RegisterExpertComponent,
    AdminExpertApplicationsListComponent,
    AdminExpertApplicationComponent,
    AdminExpertsListComponent,
    CreateNewExpertModalComponent,
    EditExpertModalComponent,
    AdminExpertApplicationComponent,
    AdminScoringProjectsComponent,
    SetExpertsModalComponent,
    ChangeEmailModalComponent,
    RoundNumberPipe,
    SearchWithAutocompleteComponent,
    ProjectListComponent,
    ScoringCostComponent,
    ScoringApplicationComponent,
    SelectComponent,
    AutocompleteComponent,
    InputSwitchComponent,
    RegisterComponent,
    ConfirmEmailComponent,
    RegisterConfirmComponent,
    WelcomeModalComponent,
    DeleteProjectModalComponent,
    ScoringApplicationComponent,
    ProjectComponent,
    ProjectAboutComponent,
    WaitingModalComponent,
    ScoringPaymentComponent,
    ExpertSelectorComponent,
    OfferDetailsComponent,
    ExpertScoringComponent
  ],
  entryComponents: [
    TransactionAwaitingModalComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    ReceiveEtherModalComponent,
    AddAdminModalComponent,
    CreateNewExpertModalComponent,
    EditExpertModalComponent,
    SetExpertsModalComponent,
    ChangeEmailModalComponent,
    ScoringCostComponent,
    WelcomeModalComponent,
    DeleteProjectModalComponent,
    WaitingModalComponent
  ],
  imports: [
    FileUploadModule,
    TableModule,
    CalendarModule,
    DataTableModule,
    FileUploadModule,
    MatCheckboxModule,
    MatSelectModule,
    MatTabsModule,
    BrowserModule,
    CheckboxModule,
    HttpClientModule,
    BrowserAnimationsModule,
    DropdownModule,
    MaterialModule,
    AppRoutingModule,
    FormsModule,
    StickyModule,
    ReactiveFormsModule,
    NgbModule.forRoot(),
    SimpleNotificationsModule.forRoot(),
    NgProgressModule,
    PaginatorModule,
    TooltipModule,
    RadioButtonModule,
    MatIconModule,
    Ng2DeviceDetectorModule.forRoot(),
    Angulartics2Module.forRoot([Angulartics2GoogleAnalytics]),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: multiTranslateLoaderFactory
      }
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NgProgressInterceptor,
      multi: true
    },
    DictionariesService,
    BalanceApiClient,
    ContractApiClient,
    AdminContractClient,
    ProjectApiClient,
    ScoringApiClient,
    EstimatesApiClient,
    ExpertApiClient,
    ExpertsRegistryContractClient,
    ScoringExpertsManagerContractClient,
    AdminApiClient,
    AuthenticationApiClient,
    AuthenticationService,
    UserContext,
    ScoreColorsService,
    ScoringCriterionService,
    Web3Service,
    DialogService,
    ScoringManagerContractClient,
    BlockiesService,
    BalanceService,
    InitializationService,
    UserApiClient,
    InitializationGuard,
    ShouldHaveEthGuard,
    ShouldBeAuthenticatedGuard,
    ShouldBeAdminGuard,
    GuardFactory,
    CompositeGuard,
    ExpertStatusGuard,
    OfferStatusGuard,
    AreaService,
    AreaService,
    EnumHelper,
    OffersApiClient,
    ScoringService,
    ProjectService,
    ScoringContractClient,
    ScoringApplicationApiClient,
    SubmittedScoringApplicationGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
