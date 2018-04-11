import {Component, ElementRef, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ActivatedRoute} from '@angular/router';
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

  constructor(private route: ActivatedRoute,
              private htmlElement: ElementRef,
              private formBuilder: FormBuilder,
              private areaService: AreaService,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private scoringCriterionService: ScoringCriterionService,
              private scoringManagerContractClient: ScoringManagerContractClient) {}

  public async ngOnInit() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.areaType = +this.route.snapshot.paramMap.get('areaType');
    this.areaTypeName = AreaType[this.areaType];
    this.areas = this.areaService.areas;

    const scoringCriterionResponse: ScoringCriteriaGroup[] = this.scoringCriterionService.getByArea(this.areaType);
    this.areasCriterion = scoringCriterionResponse;

    const scoringFields = {};
    for (const group of this.areasCriterion) {
      for (const criteria of group.criteria) {
        scoringFields['answer_' + criteria.id] = ['', [Validators.required]];
        scoringFields['comment_' + criteria.id] = ['', [Validators.required]];
      }
    }

    scoringFields['conclusion'] = ['', [Validators.required]];

    this.scoringForm = this.formBuilder.group(scoringFields);

    const projectSummary: ProjectSummaryResponse = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    this.projectName = projectSummary.name;
    this.projectExternalId = projectSummary.externalId;
  }

  private validateForm(): boolean {
    if (!this.scoringForm.invalid) {
      return true;
    }

    let invalidElName = '';

    for (let control in this.scoringForm.controls) {
        if (this.scoringForm.controls[control].status === "INVALID") {
          invalidElName = control;
          break;
        }
    }

    const invalidElement = this.htmlElement.nativeElement.querySelector('[name=' + invalidElName + ']');
    const containerOffset = invalidElement.offsetTop;
    const fieldOffset = invalidElement.offsetParent.offsetTop;

    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
  }

  public async submitFormAsync() {
    this.dialogService.showSendReportDialog();
    if (!this.validateForm()) {
      return;
    }

    const transactionHash = await this.scoringManagerContractClient.submitEstimatesAsync(
      this.projectExternalId,
      this.areaType,
      this.scoringForm.get('conclusion').value,
      this.getEstimate()
    );

    const submitRequest: SubmitEstimatesRequest = {
      transactionHash: transactionHash,
      projectId: this.projectId,
      areaType: this.areaType,
      estimateComments: this.getAnswers(),
      conclusion: this.scoringForm.get('conclusion').value
    };
    await this.estimatesApiClient.submitEstimatesAsync(submitRequest);
    this.dialogService.showSendReportDialog();
  }

  public getEstimate(): Estimate[] {
    return this.areasCriterion.map(group => group.criteria).reduce((l, r) => l.concat(r)).map(criteria => <Estimate> {
        scoringCriterionId: criteria.id,
        score: this.scoringForm.get('answer_' + criteria.id).value,
        comments: this.scoringForm.get('comment_' + criteria.id).value
    });
  }

  public getAnswers(): EstimateCommentRequest[] {
    return this.areasCriterion.map(group => group.criteria).reduce((l, r) => l.concat(r)).map(criteria => <EstimateCommentRequest>{
        scoringCriterionId: criteria.id,
        comment: this.scoringForm.get('comment_' + criteria.id).value
    });
  }

}
