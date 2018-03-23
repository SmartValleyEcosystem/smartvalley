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
import {ApplicationApiClient} from './api/application/application-api.client';
import {HeaderComponent} from './components/header/header.component';
import {MaterialModule} from './shared/material.module';
import {BalanceApiClient} from './api/balance/balance-api-client';
import {ExpertApiClient} from './api/expert/expert-api-client';
import {ApplicationComponent} from './components/application/application.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ScoringManagerContractClient} from './services/contract-clients/scoring-manager-contract-client';
import {CheckboxModule} from 'primeng/checkbox';
import {ScoringComponent} from './components/scoring/scoring.component';
import {MatTabsModule} from '@angular/material';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSelectModule} from '@angular/material/select';
import {FooterComponent} from './components/footer/footer.component';
import {SimpleNotificationsModule} from 'angular2-notifications';
import {EstimateComponent} from './components/estimate/estimate.component';
import {QuestionService} from './services/questions/question-service';
import {ContractApiClient} from './api/contract/contract-api-client';
import {ErrorInterceptor} from './api/error-interceptor';
import {AdminContractClient} from './services/contract-clients/admin-contract-client';
import {ScoringApiClient} from './api/scoring/scoring-api-client';
import {ProjectCardComponent} from './components/common/project-card/project-card.component';
import {ProjectApiClient} from './api/project/project-api-client';
import {ReportComponent} from './components/report/report.component';
import {EstimatesApiClient} from './api/estimates/estimates-api-client';
import {VotingApiClient} from './api/voting/voting-api-client';
import {UserApiClient} from './api/user/user-api-client';
import {QuestionsComponent} from './components/report/questions/questions.component';
import {CommonService} from './services/common/common.service';
import {ProjectService} from './services/project/project-service';
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
import {ProjectInformationComponent} from './components/common/project-information/project-information.component';
import {AccountComponent} from './components/account/account.component';
import {MatIconModule} from '@angular/material/icon';
import {CompositeGuard} from './services/guards/composite.guard';
import {GuardFactory} from './services/guards/guard-factory';
import {DashIfEmptyPipe} from './utils/dash-if-empty.pipe';
import {ShouldBeAuthenticatedGuard} from './services/authentication/should-be-authenticated.guard';
import {MyProjectsComponent} from './components/my-projects/my-projects.component';
import {VotingService} from './services/voting/voting-service';
import {VotingComponent} from './components/voting/voting.component';
import {VotingCardComponent} from './components/voting-card/voting-card.component';
import {VotingManagerContractClient} from './services/contract-clients/voting-manager-contract-client';
import {ScoringExpertsManagerContractClient} from './services/contract-clients/scoring-experts-manager-contract-client';
import {FreeScoringConfirmationModalComponent} from './components/common/free-scoring-confirmation-modal/free-scoring-confirmation-modal.component';
import {VotingContractClient} from './services/contract-clients/voting-contract-client';
import {VoteModalComponent} from './components/common/vote-modal/vote-modal.component';
import {CompletedVotingComponent} from './components/completed-voting/completed-voting.component';
import {CompletedVotingsComponent} from './components/completed-votings/completed-votings.component';
import {FormatDatePipe} from './utils/format-date.pipe';
import {AuthenticationApiClient} from './api/authentication/authentication-api-client';
import {RegisterModalComponent} from './components/common/register-modal/register-modal.component';
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
import {ConfirmEmailModalComponent} from './components/common/confirm-email/confirm-email-modal.component';
import {ConfirmEmailComponent} from './components/common/confirm-email/confirm-email.component';
import {ExpertsRegistryContractClient} from './services/contract-clients/experts-registry-contract-client';
import {CalendarModule} from 'primeng/calendar';
import {ExpertStatusComponent} from './components/expert-status/expert-status.component';
import {ExpertComponent} from './components/expert/expert.component';
import {RegisterExpertComponent} from './components/register-expert/register-expert.component';
import {AdminExpertApplicationsListComponent} from './components/admin-panel/admin-expert-applications-list/admin-expert-applications-list.component';
import {ExpertsCountSelectionModalComponent} from './components/common/experts-count-selection-modal/experts-count-selection-modal.component';
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
import {ExpertOffersHistoryComponent} from './components/expert/expert-offers-history/expert-offers-history.component';
import {ExpertWorkPlaceComponent} from './components/expert/expert-work-place/expert-work-place.component';
import {OffersApiClient} from './api/expert/offers-api-client';
import {ExpertOffersComponent} from './components/expert/expert-offers/expert-offers.component';
import {roundNumberPipe} from './utils/round-number.pipe';
import {SearchWithAutocompleteComponent} from './components/search-with-autocomplete/search-with-autocomplete.component';
import {ProjectListComponent} from './components/project-list/project-list.component';
import {CountryAutocompleteComponent} from './components/project-list/country-autocomplete/country-autocomplete.component';
import {CategorySelectComponent} from './components/project-list/category-select/category-select.component';
import {PaginatorModule} from 'primeng/paginator';
import {ExpertShouldBeAssignedGuard} from './services/guards/expert-should-be-assigned.guard';
import {ScoringService} from "./services/scoring/scoring.service";
import {ScoringContractClient} from "./services/contract-clients/scoring-contract-client";
import { ScoringCostComponent } from './components/common/scoring-cost-modal/scoring-cost.component';

@NgModule({
  declarations: [
    AppComponent,
    VotingComponent,
    VotingCardComponent,
    MetamaskHowtoComponent,
    LandingComponent,
    HeaderComponent,
    ApplicationComponent,
    ScoringComponent,
    FooterComponent,
    EstimateComponent,
    ProjectCardComponent,
    ReportComponent,
    QuestionsComponent,
    TransactionAwaitingModalComponent,
    ScoringCostComponent,
    NullableLinkComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    InitializationComponent,
    RootComponent,
    ReceiveEtherModalComponent,
    ProjectInformationComponent,
    AccountComponent,
    DashIfEmptyPipe,
    MyProjectsComponent,
    VotingCardComponent,
    FreeScoringConfirmationModalComponent,
    VoteModalComponent,
    RegisterModalComponent,
    CompletedVotingComponent,
    CompletedVotingsComponent,
    FormatDatePipe,
    AdminPanelComponent,
    AddAdminModalComponent,
    ExpertStatusComponent,
    ExpertComponent,
    RegisterExpertComponent,
    ConfirmEmailModalComponent,
    ConfirmEmailComponent,
    AdminExpertApplicationsListComponent,
    AdminExpertApplicationComponent,
    AdminExpertsListComponent,
    CreateNewExpertModalComponent,
    EditExpertModalComponent,
    ExpertsCountSelectionModalComponent,
    AdminExpertApplicationComponent,
    AdminScoringProjectsComponent,
    SetExpertsModalComponent,
    ChangeEmailModalComponent,
    ExpertWorkPlaceComponent,
    ExpertOffersComponent,
    ExpertOffersHistoryComponent,
    roundNumberPipe,
    SearchWithAutocompleteComponent,
    ProjectListComponent,
    ScoringCostComponent,
    CountryAutocompleteComponent,
    CategorySelectComponent
  ],
  entryComponents: [
    TransactionAwaitingModalComponent,
    AlertModalComponent,
    MetamaskManualModalComponent,
    ReceiveEtherModalComponent,
    FreeScoringConfirmationModalComponent,
    VoteModalComponent,
    RegisterModalComponent,
    AddAdminModalComponent,
    ConfirmEmailModalComponent,
    CreateNewExpertModalComponent,
    EditExpertModalComponent,
    ExpertsCountSelectionModalComponent,
    SetExpertsModalComponent,
    ChangeEmailModalComponent,
    ScoringCostComponent
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
    ReactiveFormsModule,
    NgbModule.forRoot(),
    SimpleNotificationsModule.forRoot(),
    NgProgressModule,
    PaginatorModule,
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
    CommonService,
    BalanceApiClient,
    ApplicationApiClient,
    ContractApiClient,
    AdminContractClient,
    ProjectApiClient,
    ScoringApiClient,
    EstimatesApiClient,
    ExpertApiClient,
    ExpertsRegistryContractClient,
    ScoringExpertsManagerContractClient,
    AdminApiClient,
    VotingApiClient,
    AuthenticationApiClient,
    AuthenticationService,
    UserContext,
    ProjectService,
    QuestionService,
    Web3Service,
    DialogService,
    VotingService,
    ScoringManagerContractClient,
    VotingManagerContractClient,
    VotingContractClient,
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
    ExpertShouldBeAssignedGuard,
    AreaService,
    AreaService,
    EnumHelper,
    OffersApiClient,
    ScoringService,
    ScoringContractClient
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
