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

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements AfterViewChecked, OnInit {
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
              private router: Router) {
  }

  public async ngOnInit() {
    await this.loadInitialData();
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

  private async loadInitialData(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.details = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.blockiesService.getImageForAddress(this.details.projectAddress);
    await this.reloadExpertEstimatesAsync();
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
