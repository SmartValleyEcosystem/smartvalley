import {Component, HostListener, OnDestroy, OnInit} from '@angular/core';
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
import {environment} from '../../../environments/environment';
import {DialogService} from '../../services/dialog-service';
import {NotificationsService} from 'angular2-notifications';
import {SubscriptionApiClient} from '../../api/subscription/subscription-api-client';
import {Location} from '@angular/common';
import {Subscription} from 'rxjs/Subscription';
import {ProjectService} from '../../services/project/project.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit, OnDestroy {

  public tabItems: string[] = ['about', 'application', 'report', 'allotment-events'];
  public projectId: number;
  public project: ProjectSummaryResponse;
  public editProjectsLink = Paths.ProjectEdit;
  public selectedTab = 0;
  public isAdmin: boolean;
  public routerSubscriber: Subscription;

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
              private projectService: ProjectService,
              private router: Router,
              private location: Location,
              private route: ActivatedRoute,
              private subscriptionApiClient: SubscriptionApiClient,
              private userContext: UserContext,
              private dialogService: DialogService,
              private notificationService: NotificationsService) {

    this.routerSubscriber = this.route.params.subscribe(async (params) => {
        await this.reloadProjectAsync();
    });
    this.userContext.userContextChanged.subscribe(async () => await this.reloadProjectAsync());
  }

  public async ngOnInit() {
    const currentUser = this.userContext.getCurrentUser();
    if (currentUser) {
        this.isAdmin = currentUser.isAdmin;
    }
    await this.reloadProjectAsync();
  }

  private async reloadProjectAsync() {
      const newProjectId = +this.route.snapshot.paramMap.get('id');
      if (!isNullOrUndefined(this.projectId) && this.projectId === newProjectId) {
      return;
    }

    this.projectId = newProjectId;
    const selectedTabName = this.route.snapshot.paramMap.get('tab');

    try {
      this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
      if (this.project.scoring.scoringStatus === ScoringStatus.InProgress) {
        this.scoringCompletenessInPercents = this.getScoringCompleteness(this.project.scoring);
      }

      if (!isNullOrUndefined(this.project.scoringStartTransactionHash)) {
        this.scoringStartTransactionUrl = this.projectService.getTransactionUrl(this.project.scoringStartTransactionHash);
      }

      const currentUser = await this.userContext.getCurrentUser();
      if (!isNullOrUndefined(currentUser) && this.project.authorId === currentUser.id) {
        this.isAuthor = true;
      }

      if (!isNullOrUndefined(selectedTabName) && this.tabItems.includes(selectedTabName)) {
          if (this.project.isPrivate && !this.isAdmin && selectedTabName === 'report') {
            this.location.replaceState(Paths.Project + '/' + this.projectId + '/details/' + this.tabItems[0]);
          } else {
            this.selectedTab = this.tabItems.indexOf(selectedTabName);
          }
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
    return !this.project.isPrivate || this.project.scoring.scoringStatus === ScoringStatus.Finished || this.isAdmin;
  }

  public reportTabClass(): string {
    return this.isReportTabAvailable() === true ? '' : 'hidden';
  }

  public getScoringStatus(): ScoringStatus {
    const scoringStatus =  this.projectService.getScoringStatus(this.project.scoring.scoringStatus, this.project.scoringStartTransactionStatus, this.project.isApplicationSubmitted);
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

  public ngOnDestroy() {
      this.routerSubscriber.unsubscribe();
  }
}
