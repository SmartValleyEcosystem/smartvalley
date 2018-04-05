import {Component, QueryList, ViewChildren, ElementRef, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ScoringCriterionService} from '../../services/criteria/scoring-criterion.service';
import {Paths} from '../../paths';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {EstimateCommentRequest} from '../../api/estimates/estimate-comment-request';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {AreaType} from '../../api/scoring/area-type.enum';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {DialogService} from '../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {Estimate} from '../../services/estimate';
import {BalanceService} from '../../services/balance/balance.service';
import {ScoringManagerContractClient} from '../../services/contract-clients/scoring-manager-contract-client';
import {UserContext} from '../../services/authentication/user-context';
import {ScoringCriterion} from '../../services/scoring-criterion';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {
  public areaType: AreaType;
  public projectId: number;
  public projectDetails: ProjectDetailsResponse;
  public estimateForm: FormGroup;
  public isEstimateSending: boolean;

  @ViewChildren('required') public requiredFields: QueryList<any>;

  constructor(private route: ActivatedRoute,
              private projectApiClient: ProjectApiClient,
              private scoringCriterionService: ScoringCriterionService,
              private router: Router,
              private estimatesApiClient: EstimatesApiClient,
              private userContext: UserContext,
              private formBuilder: FormBuilder,
              private scoringContractClient: ScoringManagerContractClient,
              private dialogService: DialogService,
              private translateService: TranslateService,
              private balanceService: BalanceService) {
  }

  public async ngOnInit(): Promise<void> {
    await this.loadProjectDetailsAsync();
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
      areaType: this.areaType,
      expertAddress: this.userContext.getCurrentUser().account,
      estimateComments: estimates.map(e => <EstimateCommentRequest>{
        scoringCriterionId: e.scoringCriterionId,
        score: e.score,
        comment: e.comments
      })
    };

    return this.estimatesApiClient.submitEstimatesAsync(submitEstimatesRequest);
  }

  private async submitToContractAsync(estimates: Array<Estimate>): Promise<string> {
    try {
      return await this.scoringContractClient.submitEstimatesAsync(
        this.projectDetails.externalId,
        this.areaType,
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
    return this.estimateForm.value.criteria.map(q => <Estimate>{
      scoringCriterionId: q.scoringCriterionId,
      score: q.score,
      comments: q.comments.replace(/\s+$/, '')
    });
  }

  private async loadProjectDetailsAsync(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.areaType = +this.route.snapshot.queryParamMap.get('areaType');

    const criteria = this.scoringCriterionService.getByArea(this.areaType);
    const criteriaFormGroups = criteria.map(q => this.createCriterionFormGroup(q));
    this.estimateForm = this.formBuilder.group({criteria: this.formBuilder.array(criteriaFormGroups)});
    this.projectDetails = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
  }

  private createCriterionFormGroup(scoringCriterion: ScoringCriterion): FormGroup {
    return this.formBuilder.group({
      scoringCriterionId: scoringCriterion.id,
      description: scoringCriterion.description,
      score: ['', [
        Validators.required,
        Validators.max(2),
        Validators.min(0),
        Validators.pattern('^-*[0-9]+$')]],
      comments: ['', [Validators.required, Validators.maxLength(250)]],
    });
  }
}
