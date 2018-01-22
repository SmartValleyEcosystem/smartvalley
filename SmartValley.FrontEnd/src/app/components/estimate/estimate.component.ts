import {Component, QueryList, ViewChildren, ElementRef, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {QuestionService} from '../../services/questions/question-service';
import {Paths} from '../../paths';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {EstimateCommentRequest} from '../../api/estimates/estimate-comment-request';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {DialogService} from '../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {Estimate} from '../../services/estimate';
import {BalanceService} from '../../services/balance/balance.service';
import {QuestionResponse} from '../../api/estimates/question-response';
import {ScoringManagerContractClient} from '../../services/contract-clients/scoring-manager-contract-client';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {
  public hidden: boolean;
  public expertiseArea: ExpertiseArea;
  public projectId: number;
  public projectDetails: ProjectDetailsResponse;
  public estimateForm: FormGroup;
  public isEstimateSending: boolean;

  @ViewChildren('required') public requiredFields: QueryList<any>;

  constructor(private route: ActivatedRoute,
              private projectApiClient: ProjectApiClient,
              private questionService: QuestionService,
              private router: Router,
              private estimatesApiClient: EstimatesApiClient,
              private authenticationService: AuthenticationService,
              private formBuilder: FormBuilder,
              private scoringContractClient: ScoringManagerContractClient,
              private dialogService: DialogService,
              private translateService: TranslateService,
              private balanceService: BalanceService) {
  }

  public ngOnInit(): void {
    this.loadProjectDetailsAsync();
  }

  public changeHidden() {
    this.hidden = true;
  }

  public async sendEstimateAsync(): Promise<void> {
    if (!this.validateForm()) {
      return;
    }

    this.isEstimateSending = true;

    const isSucceeded = await this.submitAsync();
    if (isSucceeded) {
      await this.router.navigate([Paths.Scoring]);
    }
  }

  private validateForm(): boolean {
    if (!this.estimateForm.invalid) {
      return true;
    }

    const invalidElements = this.requiredFields.filter(i => i.nativeElement.classList.contains('ng-invalid'));
    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        this.setInvalid(invalidElements[a]);
      }
      this.scrollToElement(invalidElements[0]);
    }
    return false;
  }

  private async submitAsync(): Promise<boolean> {
    const estimates = this.getEstimates();

    const transactionHash = await this.submitToContractAsync(estimates);
    if (transactionHash == null) {
      this.isEstimateSending = false;
      return false;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('Estimate.Dialog'),
      transactionHash
    );
    await this.submitToBackendAsync(transactionHash, estimates);

    await this.balanceService.updateBalanceAsync();

    transactionDialog.close();
    return true;
  }

  private submitToBackendAsync(transactionHash: string, estimates: Array<Estimate>) {
    const submitEstimatesRequest = <SubmitEstimatesRequest>{
      transactionHash: transactionHash,
      projectId: this.projectId,
      expertAddress: this.authenticationService.getCurrentUser().account,
      estimateComments: estimates.map(e => <EstimateCommentRequest>{
        questionId: e.questionId,
        score: e.score,
        comment: e.comments
      })
    };

    return this.estimatesApiClient.submitEstimatesAsync(submitEstimatesRequest);
  }

  private async submitToContractAsync(estimates: Array<Estimate>): Promise<string> {
    try {
      return await this.scoringContractClient.submitEstimatesAsync(
        this.projectDetails.projectAddress,
        this.expertiseArea,
        estimates);
    } catch (e) {
      return null;
    }
  }

  private setInvalid(element: ElementRef) {
    element.nativeElement.classList.add('ng-invalid');
    element.nativeElement.classList.add('ng-dirty');
  }

  private scrollToElement(element: ElementRef) {
    const offsetTop1 = element.nativeElement.offsetTop;
    const offsetTop3 = element.nativeElement.offsetParent.offsetParent.offsetTop;
    const offsetTop4 = element.nativeElement.offsetParent.offsetParent.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: offsetTop1 + offsetTop3 + offsetTop4, behavior: 'smooth'});
  }

  private getEstimates(): Array<Estimate> {
    return this.estimateForm.value.questions.map(q => <Estimate>{
      questionId: q.questionId,
      score: q.score,
      comments: q.comments.replace(/\s+$/, '')
    });
  }

  private async loadProjectDetailsAsync(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.expertiseArea = +this.route.snapshot.queryParamMap.get('expertiseArea');

    const questions = this.questionService.getByExpertiseArea(this.expertiseArea);
    const questionsFormGroups = questions.map(q => this.createQuestionFormGroup(q));
    this.estimateForm = this.formBuilder.group({questions: this.formBuilder.array(questionsFormGroups)});
    this.projectDetails = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
  }

  private createQuestionFormGroup(question: QuestionResponse): FormGroup {
    return this.formBuilder.group({
      questionId: question.id,
      name: question.name,
      description: question.description,
      maxScore: question.maxScore,
      score: ['', [
        Validators.required,
        Validators.max(question.maxScore),
        Validators.min(question.minScore),
        Validators.pattern('^-*[0-9]+$')]],
      comments: ['', [Validators.required, Validators.maxLength(250)]],
    });
  }
}
