import {AfterViewChecked, Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {QuestionService} from '../../services/questions/question-service';
import {Question} from '../../services/questions/question';
import {Estimate} from '../../services/estimate';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {QuestionResponse} from '../../api/estimates/question-response';
import {QuestionWithEstimatesResponse} from '../../api/estimates/question-with-estimates-response';
import {VotingStatus} from '../../services/voting-status.enum';
import {ScoringStatus} from '../../services/scoring-status.enum';
import {isNullOrUndefined} from 'util';
import * as timespan from 'timespan';
import {ScoringManagerContractClient} from '../../services/contract-clients/scoring-manager-contract-client';
import {BalanceService} from '../../services/balance/balance.service';
import {TranslateService} from '@ngx-translate/core';
import {DialogService} from '../../services/dialog-service';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {NotificationsService} from 'angular2-notifications';

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

  public questions: Array<Question>;
  public details: ProjectDetailsResponse;
  public expertiseAreaAverageScore: number;
  public expertiseMaxScore: number;
  public projectImageUrl: string;

  private projectId: number;
  @ViewChild('reportTabSet')
  private reportTabSet: NgbTabset;
  private selectedReportTab: string;
  private selectedExpertiseTabIndex: number;
  private knownTabs = [Constants.ReportFormTab, Constants.ReportEstimatesTab];

  constructor(private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private translateService: TranslateService,
              private balanceService: BalanceService,
              private dialogService: DialogService,
              private scoringApiClient: ScoringApiClient,
              private notificationsService: NotificationsService,
              private scoringManagerContractClient: ScoringManagerContractClient) {
  }

  public async ngOnInit() {
    await this.loadInitialDataAsync();
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

  public async onExpertiseTabIndexChanged(index: number) {
    this.selectedExpertiseTabIndex = index;
    await this.reloadExpertEstimatesAsync();
  }

  public showEstimates() {
    this.reportTabSet.select('estimates');
  }

  public showForm() {
    this.reportTabSet.select('about');
  }

  public async submitToScoringAsync(): Promise<void> {
    const transactionHash = await this.startScoringAsync(this.details.externalId);
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

    await this.scoringApiClient.startAsync(this.details.externalId, transactionHash);
    await this.balanceService.updateBalanceAsync();

    transactionDialog.close();
  }

  private async startScoringAsync(projectId: string): Promise<string> {
    try {
      return await this.scoringManagerContractClient.startAsync(projectId);
    } catch (e) {
      return null;
    }
  }

  private async loadInitialDataAsync(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.details = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.getImageUrl();
    if (this.details.scoringStatus !== ScoringStatus.Pending) {
      await this.reloadExpertEstimatesAsync();
    } else if (this.details.votingStatus !== VotingStatus.InProgress) {
      this.scoringCost = +(await this.scoringManagerContractClient.getScoringCostAsync()).toFixed(3);
    } else {
      this.updateVotingRemainingTime();
      setInterval(() => this.updateVotingRemainingTime(), 1000);
    }
  }

  private updateVotingRemainingTime(): void {
    if (isNullOrUndefined(this.details.votingEndDate)) {
      return;
    }

    const remaining = timespan.fromDates(new Date(), this.details.votingEndDate);
    this.votingRemainingDays = remaining.days;
    this.votingRemainingHours = remaining.hours;
    this.votingRemainingMinutes = remaining.minutes;
    this.votingRemainingSeconds = remaining.seconds;
  }

  private getImageUrl(): string {
    const address = this.details.projectAddress ? this.details.projectAddress : this.details.authorAddress;
    return this.blockiesService.getImageForAddress(address);
  }

  private async reloadExpertEstimatesAsync(): Promise<void> {
    const expertiseArea = this.getExpertiseAreaByIndex(this.selectedExpertiseTabIndex);
    const estimatesResponse = await this.estimatesApiClient.getAsync(this.projectId, expertiseArea);
    const questionsInArea = this.questionService.getByExpertiseArea(expertiseArea);

    this.expertiseAreaAverageScore = estimatesResponse.averageScore;
    this.expertiseMaxScore = this.questionService.getMaxScoreForExpertiseArea(expertiseArea);
    this.questions = estimatesResponse.questions.map(q => this.createQuestion(questionsInArea, q));
  }

  private createQuestion(questionsInArea: Array<QuestionResponse>, questionWithEstimates: QuestionWithEstimatesResponse): Question {
    const questionId = questionWithEstimates.questionId;
    const question = questionsInArea.filter(j => j.id === questionId)[0];

    return <Question>{
      name: question.name,
      description: question.description,
      minScore: question.minScore,
      maxScore: question.maxScore,
      estimates: questionWithEstimates.estimates.map(j => <Estimate>{
        questionId: questionId,
        score: j.score,
        comments: j.comment
      })
    };
  }

  private getExpertiseAreaByIndex(index: number): ExpertiseArea {
    switch (index) {
      case 1 :
        return ExpertiseArea.Lawyer;
      case 2 :
        return ExpertiseArea.Analyst;
      case 3 :
        return ExpertiseArea.TechnicalExpert;
      default:
        return ExpertiseArea.HR;
    }
  }
}
