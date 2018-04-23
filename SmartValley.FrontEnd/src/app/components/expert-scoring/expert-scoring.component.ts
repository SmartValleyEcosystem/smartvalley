import {Component, ElementRef, OnInit, QueryList, ViewChildren} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {ScoringCriterionService} from '../../services/criteria/scoring-criterion.service';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {Area} from '../../services/expert/area';
import {AreaService} from '../../services/expert/area.service';
import {ScoringCriteriaGroup} from '../../services/criteria/scoring-criteria-group';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AreaType} from '../../api/scoring/area-type.enum';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {ScoringManagerContractClient} from '../../services/contract-clients/scoring-manager-contract-client';
import {EstimateCommentRequest} from '../../api/estimates/estimate-comment-request';
import {Estimate} from '../../services/estimate';
import {DialogService} from '../../services/dialog-service';
import {CriterionPromptResponse} from '../../api/estimates/criterion-prompt-response';
import {CriterionPrompt} from '../../api/estimates/criterion-prompt';
import {Paths} from '../../paths';
import {TranslateService} from '@ngx-translate/core';
import {SaveEstimatesRequest} from '../../api/estimates/save-estimates-request';
import {QuestionControlType} from '../../api/scoring-application/question-control-type.enum';

@Component({
  selector: 'app-expert-scoring',
  templateUrl: './expert-scoring.component.html',
  styleUrls: ['./expert-scoring.component.scss']
})
export class ExpertScoringComponent implements OnInit {

  public projectName = '';
  public projectId: number;
  public projectExternalId: string;
  public areaType: number;
  public areaTypeName: string;
  public areas: Area[];
  public areasCriterion: ScoringCriteriaGroup[] = [];
  public scoringForm: FormGroup;
  public questionsActivity: Array<boolean> = [];
  public criterionPrompts: CriterionPromptResponse[];
  public questionTypeComboBox = QuestionControlType.Combobox;
  public isSaved = false;
  public saveTime: string

  @ViewChildren('required') public requiredFields: QueryList<any>;

  constructor(private route: ActivatedRoute,
              private htmlElement: ElementRef,
              private formBuilder: FormBuilder,
              private areaService: AreaService,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private scoringCriterionService: ScoringCriterionService,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private translateService: TranslateService,
              private router: Router) {
  }

  public async ngOnInit() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.areaType = +this.route.snapshot.paramMap.get('areaType');
    this.areaTypeName = AreaType[this.areaType];
    this.areas = this.areaService.areas;

    const scoringCriterionResponse: ScoringCriteriaGroup[] = this.scoringCriterionService.getByArea(this.areaType);
    this.areasCriterion = scoringCriterionResponse;

    const scoringFields = {};
    this.areasCriterion.selectMany(group => group.criteria)
      .map(criterion => {
        scoringFields['answer_' + criterion.id] = ['', [Validators.required]];
        scoringFields['comment_' + criterion.id] = ['', [Validators.required]];
      });

    scoringFields['conclusion'] = ['', [Validators.required]];

    this.scoringForm = this.formBuilder.group(scoringFields);

    const projectSummary: ProjectSummaryResponse = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    this.projectName = projectSummary.name;
    this.projectExternalId = projectSummary.externalId;

    const draftData = await this.estimatesApiClient.getEstimatesDraftAsync(this.projectId);
    this.addDraftData(draftData);

    const criterionPromptsResponse = await this.estimatesApiClient.getCriterionPromptsAsync(this.projectId, this.areaType);
    this.criterionPrompts = criterionPromptsResponse.items;
    this.questionsActivity = [true];
  }

  public addDraftData(data) {
    const prepareFormData = {};
    if (!data.estimates.length) {
      return;
    }
    for (const estimate of data.estimates) {
      prepareFormData['answer_' + estimate.scoringCriterionId] = estimate.score;
      prepareFormData['comment_' + estimate.scoringCriterionId] = estimate.comment;
    }

    prepareFormData['conclusion'] = data.conclusion;
    this.scoringForm.setValue(prepareFormData);
  }

  public getCriterionInfo(id): CriterionPrompt[] {
    return this.criterionPrompts.find((c) => c.scoringCriterionId === id).prompts;
  }

  private validateForm(): boolean {
    const invalidElements = this.requiredFields.filter(
      (i) => {
        let elem = false;

        if (i.el) {
          elem = i.el.nativeElement.classList.contains('ng-invalid');
        }

        if (i.nativeElement) {
          if (i.nativeElement.nativeElement) {
            return i.nativeElement.nativeElement.classList.contains('ng-invalid');
          }
          elem = i.nativeElement.classList.contains('ng-invalid');
        }
        return elem;
      });

    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        const element = invalidElements[a].el !== undefined ? invalidElements[a].el : invalidElements[a];
        this.setInvalid(element);
      }
      const firstElement = invalidElements[0].el !== undefined ? invalidElements[0].el : invalidElements[0];
      this.scrollToElement(firstElement);
      return false;
    }

    return true;
  }

  private scrollToElement(element: ElementRef): void {
    if (element.nativeElement.nativeElement) {
      const offsetTop1 = element.nativeElement.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
      return;
    }
    if (element.nativeElement) {
      const offsetTop1 = element.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
    }
  }

  private setInvalid(element: ElementRef): void {
    if (element.nativeElement.nativeElement) {
      element.nativeElement.nativeElement.classList.add('ng-invalid');
      element.nativeElement.nativeElement.classList.add('ng-dirty');
      return;
    }
    if (element.nativeElement) {
      element.nativeElement.classList.add('ng-invalid');
      element.nativeElement.classList.add('ng-dirty');
    }
  }

  public async submitFormAsync() {
    if (!this.validateForm()) {
      return;
    }

    this.dialogService.showSendReportDialog();
    const transactionHash = await this.scoringManagerContractClient.submitEstimatesAsync(
      this.projectExternalId,
      this.areaType,
      this.scoringForm.get('conclusion').value,
      this.getEstimate()
    );

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('OfferDetails.Dialog'),
      transactionHash
    );

    const submitRequest: SubmitEstimatesRequest = {
      transactionHash: transactionHash,
      projectId: this.projectId,
      areaType: this.areaType,
    };
    await this.estimatesApiClient.submitEstimatesAsync(submitRequest);
    transactionDialog.close();
    this.dialogService.showSendReportDialog();
    await this.router.navigate([Paths.ScoringList]);
  }

  public getEstimate(): Estimate[] {
    return this.areasCriterion.selectMany(group => group.criteria)
      .map(criteria => <Estimate> {
        scoringCriterionId: criteria.id,
        score: this.scoringForm.get('answer_' + criteria.id).value === '' ? null : +this.scoringForm.get('answer_' + criteria.id).value,
        comments: this.scoringForm.get('comment_' + criteria.id).value
      });
  }

  public getAnswers(): EstimateCommentRequest[] {
    return this.areasCriterion.selectMany(group => group.criteria)
      .map(criteria => <EstimateCommentRequest>{
        scoringCriterionId: criteria.id,
        comment: this.scoringForm.get('comment_' + criteria.id).value,
        score: this.scoringForm.get('answer_' + criteria.id).value === '' ? null : +this.scoringForm.get('answer_' + criteria.id).value,
      });
  }

  public chageActiveQuestion(id) {
    this.questionsActivity = this.questionsActivity.map((q, i) => i === id ? this.questionsActivity[id] : false);
    this.questionsActivity[id] = !this.questionsActivity[id];
  }

  public async saveDraft() {
    const draftRequest: SaveEstimatesRequest = {
      projectId: this.projectId,
      areaType: this.areaType,
      estimateComments: this.getAnswers(),
      conclusion: this.scoringForm.get('conclusion').value
    };

    await this.estimatesApiClient.saveEstimatesAsync(draftRequest);
    this.isSaved = true;
    const currentDate = new Date();
    const options = {
      hour: 'numeric',
      minute: 'numeric',
    };
    this.saveTime = currentDate.toLocaleString(navigator.language, options);
  }

  public getProjectApplictionLink() {
    return Paths.Project + '/' + this.projectId + '/details/application';
  }

  async navigateToProjectApplication() {
    await this.router.navigate([Paths.Project + '/' + this.projectId + '/details/application']);
  }
}
