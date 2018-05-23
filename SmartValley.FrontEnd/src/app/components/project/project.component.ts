import {Component, HostListener, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {UserContext} from '../../services/authentication/user-context';
import {isNullOrUndefined} from 'util';
import {ScoringStatus} from '../../services/scoring-status.enum';
import {OfferStatus} from '../../api/scoring-offer/offer-status.enum';
import {ScoringResponse} from '../../api/scoring/scoring-response';
import {ErrorCode} from '../../shared/error-code.enum';
import {ScoringStartTransactionStatus} from '../../api/project/scoring-start-transaction.status';
import {environment} from '../../../environments/environment';
import {DialogService} from '../../services/dialog-service';
import {NotificationsService} from 'angular2-notifications';
import {SubscriptionApiClient} from '../../api/subscription/subscription-api-client';
import {Location} from '@angular/common';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  public tabItems: string[] = ['about', 'application', 'report'];
  public projectId: number;
  public project: ProjectSummaryResponse;
  public editProjectsLink = Paths.ProjectEdit;
  public selectedTab = 0;

  public ScoringStatus = ScoringStatus;

  public isAuthor = false;
  public scoringCompletenessInPercents;
  public scoringStartTransactionUrl = '';

  private currentGlobalOffset = 0;

  @HostListener('window:scroll', ['$event'])
  doSomething(event) {
    if (event.target.scrollingElement.scrollTop === 0) {
      event.target.scrollingElement.scrollTop = this.currentGlobalOffset;
      return;
    }
    this.currentGlobalOffset = window.pageYOffset;
  }

  constructor(private projectApiClient: ProjectApiClient,
              private router: Router,
              private location: Location,
              private route: ActivatedRoute,
              private subscriptionApiClient: SubscriptionApiClient,
              private userContext: UserContext,
              private dialogService: DialogService,
              private notificationService: NotificationsService) {

    this.route.params.subscribe(async (params) => {
        await this.reloadProjectAsync();
    });
    this.userContext.userContextChanged.subscribe(async () => await this.reloadProjectAsync());
  }

  public async ngOnInit() {
    await this.reloadProjectAsync();
  }

  private async reloadProjectAsync() {
    const newProjectId = +this.route.snapshot.paramMap.get('id');
    if (!isNullOrUndefined(this.projectId) && this.projectId === newProjectId) {
      return;
    }

    this.projectId = newProjectId;
    const selectedTabName = this.route.snapshot.paramMap.get('tab');
    if (!isNullOrUndefined(selectedTabName) && this.tabItems.includes(selectedTabName)) {
      this.selectedTab = this.tabItems.indexOf(selectedTabName);
    }

    try {
      this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
      if (this.project.scoring.scoringStatus === ScoringStatus.InProgress) {
        this.scoringCompletenessInPercents = this.getScoringCompleteness(this.project.scoring);
      }

      if (!isNullOrUndefined(this.project.scoringStartTransactionHash)) {
        this.scoringStartTransactionUrl = `${environment.etherscan_host}/tx/${this.project.scoringStartTransactionHash}`;
      }

      const currentUser = await this.userContext.getCurrentUser();
      if (!isNullOrUndefined(currentUser) && this.project.authorId === currentUser.id) {
        this.isAuthor = true;
      }
    } catch (e) {
      switch (e.error.errorCode) {
        case ErrorCode.ProjectNotFound:
          await this.router.navigate([Paths.ProjectList]);
      }
    }
  }

  public async navigateToApplicationScoringAsync(): Promise<void> {
    await this.router.navigate([Paths.ScoringApplication + '/' + this.projectId]);
  }

  public async navigateToPaymentAsync(): Promise<void> {
    await this.router.navigate([Paths.Project + '/' + this.projectId + '/payment']);
  }

  private getScoringCompleteness(scoring: ScoringResponse): number {
    const finishedOffers = scoring.offers.filter(o => o.status === OfferStatus.Finished).length;
    const totalOffers = scoring.requiredExpertsCount;
    return Math.round(finishedOffers * 100 / totalOffers);
  }

  public async showSubscribeDialog() {
    const subscribe = await this.dialogService.showSubscribeDialog();
    if (subscribe) {
      await this.subscriptionApiClient.subscribeAsync(subscribe, this.projectId);
      this.notificationService.success('Success', 'Subscribe request is sent');
    }
  }

  public onChangeTab($event) {
      this.location.replaceState(Paths.Project + '/' + this.projectId + '/details/' + this.tabItems[$event.index]);
  }

  public isReportTabAvailable(): boolean {
    return !this.project.isPrivate || this.project.scoring.scoringStatus === ScoringStatus.Finished;
  }

  public getScoringStatus(): ScoringStatus {
    const scoringStatus = this.getOriginalScoringStatus();
    const hiddenPrivateScoringStatuses = [
      ScoringStatus.ReadyForPayment,
      ScoringStatus.PaymentInProcess,
      ScoringStatus.PaymentFailed
    ];

    if (this.project.isPrivate && this.isAuthor && hiddenPrivateScoringStatuses.includes(scoringStatus)) {
      return ScoringStatus.InProgress;
    } else {
      return scoringStatus;
    }
  }

  private getOriginalScoringStatus(): ScoringStatus {
    if (this.project.scoring.scoringStatus === ScoringStatus.FillingApplication) {
      if (this.project.scoringStartTransactionStatus === ScoringStartTransactionStatus.NotSubmitted) {
        if (this.project.isApplicationSubmitted) {
          return ScoringStatus.ReadyForPayment;
        } else {
          return ScoringStatus.FillingApplication;
        }
      }

      if (this.project.scoringStartTransactionStatus === ScoringStartTransactionStatus.InProgress) {
        return ScoringStatus.PaymentInProcess;
      }

      if (this.project.scoringStartTransactionStatus === ScoringStartTransactionStatus.Failed) {
        return ScoringStatus.PaymentFailed;
      }
    }

    return this.project.scoring.scoringStatus;
  }
}
