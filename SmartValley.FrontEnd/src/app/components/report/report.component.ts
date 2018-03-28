import {AfterViewChecked, Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ScoringCriterionService} from '../../services/criteria/scoring-criterion.service';
import {CriterionWithEstimates} from '../../services/criteria/criterionWithEstimates';
import {Estimate} from '../../services/estimate';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ScoreColorsService} from '../../services/project/score-colors.service';
import {BlockiesService} from '../../services/blockies-service';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {CriterionWithEstimatesResponse} from '../../api/estimates/criterion-with-estimates-response';
import {VotingStatus} from '../../services/voting-status.enum';
import {ScoringStatus} from '../../services/scoring-status.enum';
import {isNullOrUndefined} from 'util';
import * as timespan from 'timespan';
import {BalanceService} from '../../services/balance/balance.service';
import {TranslateService} from '@ngx-translate/core';
import {DialogService} from '../../services/dialog-service';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {NotificationsService} from 'angular2-notifications';
import * as moment from 'moment';
import {UserContext} from '../../services/authentication/user-context';
import {AreaService} from '../../services/expert/area.service';
import {Area} from '../../services/expert/area';
import {ScoringService} from '../../services/scoring/scoring.service';
import {ScoringCriterion} from '../../services/scoring-criterion';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements AfterViewChecked, OnInit {
  public ScoringStatus = ScoringStatus;
  public VotingStatus = VotingStatus;

  public votingRemainingDays: number;
  public votingRemainingHours: number;
  public votingRemainingMinutes: number;
  public votingRemainingSeconds: number;
  public scoringCost: number;

  public criteriaWithEstimates: Array<CriterionWithEstimates>;
  public details: ProjectDetailsResponse;
  public areaScore: number;
  public areaMaxScore: number;
  public currentAccount: string;
  public areas: Area[];

  private projectId: number;
  @ViewChild('reportTabSet')
  private reportTabSet: NgbTabset;
  public selectedReportTab: string;
  public selectedAreaTabIndex: number;
  private knownTabs = [Constants.ReportFormTab, Constants.ReportEstimatesTab];

  constructor(private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private scoringCriterionService: ScoringCriterionService,
              private route: ActivatedRoute,
              private blockiesService: BlockiesService,
              public scoreColorsService: ScoreColorsService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private translateService: TranslateService,
              private balanceService: BalanceService,
              private dialogService: DialogService,
              private scoringApiClient: ScoringApiClient,
              private notificationsService: NotificationsService,
              private userContext: UserContext,
              private scoringService: ScoringService,
              private areaService: AreaService) {
  }

  public async ngOnInit() {
    this.selectedAreaTabIndex = 0;
    await this.loadInitialDataAsync();
    const currentUser = this.userContext.getCurrentUser();
    if (!isNullOrUndefined(currentUser)) {
      this.currentAccount = currentUser.account;
    }
    this.activatedRoute.queryParams.subscribe(params => {
      this.selectedReportTab = params[Constants.TabQueryParam];
    });
  }

  public ngAfterViewChecked(): void {
    if (this.reportTabSet && this.knownTabs.includes(this.selectedReportTab)) {
      this.reportTabSet.select(this.selectedReportTab);
    }
  }

  public async onMainTabChanged($event: any) {
    const queryParams = Object.assign({}, this.activatedRoute.snapshot.queryParams);
    queryParams[Constants.TabQueryParam] = $event.nextId;
    await this.router.navigate([Paths.Report + '/' + this.projectId], {queryParams: queryParams, replaceUrl: true});
  }

  public async onAreaTabIndexChanged(index: number) {
    this.selectedAreaTabIndex = index;
    await this.reloadExpertEstimatesAsync();
  }

  public showEstimates() {
    this.reportTabSet.select('estimates');
  }

  public showForm() {
    this.reportTabSet.select('about');
  }

  public async submitToScoringAsync(): Promise<void> {
    const areas = this.areaService.areas.map(a => a.areaType);
    const areaExpertCounts = await this.dialogService.showExpertsCountSelectionDialogAsync(areas);

    const transactionHash = await this.startScoringAsync(areas, areaExpertCounts);
    if (transactionHash == null) {
      this.notificationsService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Common.TryAgain'));
      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('Application.Dialog'),
      transactionHash
    );

    await this.scoringApiClient.startAsync(this.details.externalId, areas, areaExpertCounts, transactionHash);
    await this.balanceService.updateBalanceAsync();

    transactionDialog.close();
  }

  private async startScoringAsync(areas: Array<number>,
                                  areaExpertCounts: Array<number>): Promise<string> {
    try {
      if (this.details.votingStatus === VotingStatus.Accepted) {
        return await this.scoringService.startForFreeAsync(
          this.details.externalId,
          this.details.votingAddress,
          areas,
          areaExpertCounts);
      } else {
        return await this.scoringService.startAsync(
          this.details.externalId,
          areas,
          areaExpertCounts);
      }
    } catch (e) {
      return null;
    }
  }

  private async loadInitialDataAsync(): Promise<void> {
    this.areas = this.areaService.areas;
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.details = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    if (this.details.scoringStatus !== ScoringStatus.Pending) {
      await this.reloadExpertEstimatesAsync();
    } else if (this.details.votingStatus !== VotingStatus.InProgress) {
      this.scoringCost = +(await this.scoringService.getScoringCostEthByAddressAsync(this.details.scoringContractAddress)).toFixed(3);
    } else {
      this.updateVotingRemainingTime();
      setInterval(() => this.updateVotingRemainingTime(), 1000);
    }
  }

  private updateVotingRemainingTime(): void {
    if (isNullOrUndefined(this.details.votingEndDate)) {
      return;
    }

    const remaining = timespan.fromDates(new Date(), moment(this.details.votingEndDate).toDate());
    this.votingRemainingDays = remaining.days;
    this.votingRemainingHours = remaining.hours;
    this.votingRemainingMinutes = remaining.minutes;
    this.votingRemainingSeconds = remaining.seconds;
  }

  private async reloadExpertEstimatesAsync(): Promise<void> {
    const areaType = this.areaService.getTypeByIndex(this.selectedAreaTabIndex);
    const estimatesResponse = await this.estimatesApiClient.getAsync(this.projectId, areaType);
    const criteria = this.scoringCriterionService.getByArea(areaType);

    this.areaScore = estimatesResponse.score;
    this.areaMaxScore = this.areaService.getMaxScore(areaType);
    this.criteriaWithEstimates = estimatesResponse.criteria.map(q => this.createCriterionWithEstimates(criteria, q));
  }

  private createCriterionWithEstimates(scoringCriteria: Array<ScoringCriterion>,
                                       criterionWithEstimatesResponse: CriterionWithEstimatesResponse): CriterionWithEstimates {
    const scoringCriterionId = criterionWithEstimatesResponse.scoringCriterionId;
    const scoringCriterion = scoringCriteria.filter(j => j.id === scoringCriterionId)[0];

    return <CriterionWithEstimates>{
      description: scoringCriterion.description,
      estimates: criterionWithEstimatesResponse.estimates.map(j => <Estimate>{
        scoringCriterionId: scoringCriterionId,
        score: j.score,
        comments: j.comment
      })
    };
  }
}
