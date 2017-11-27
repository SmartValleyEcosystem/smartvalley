import {Component} from '@angular/core';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ActivatedRoute} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {Estimate} from '../../services/estimate';
import {isNullOrUndefined} from 'util';


@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  public questions: Array<Question>;
  report: ProjectDetailsResponse;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;
  private projectId: number;

  constructor(private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute) {
    this.loadInitialData();
  }

  tabChanged($event: any) {
    let scoringCategory: ScoringCategory = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        scoringCategory = ScoringCategory.HR;
        break;
      case 1 :
        scoringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scoringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scoringCategory = ScoringCategory.TechnicalExpert;
        break;
    }
    this.loadExpertEstimates(scoringCategory);
  }

  private async loadInitialData() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.report = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.loadExpertEstimates(ScoringCategory.HR);
  }

  private async loadExpertEstimates(scoringCategory: ScoringCategory) {
    const estimatesResponse = await this.estimatesApiClient.getByProjectIdAndCategoryAsync({
      projectId: this.projectId,
      category: scoringCategory
    });

    const questions = this.questionService.getByExpertType(scoringCategory);

    for (const question of questions) {
      question.estimates = [];
      const estimates = estimatesResponse.items.filter(e => e.questionIndex === question.indexInCategory);
      for (const estimate of estimates) {
        if (isNullOrUndefined(estimate)) {
          continue;
        }
        question.estimates.push(<Estimate>{
          score: estimate.score,
          comments: estimate.comment
        });
      }

    }

    this.questions = questions;
  }

  private colorOfProjectRate(rate: number): string {
    if (rate == null) {
      return '';
    }
    if (rate > 80) {
      return 'high_rate';
    }
    if (rate > 45) {
      return 'medium_rate';
    }
    if (rate >= 0) {
      return 'low_rate';
    }
    return 'progress_rate';
  }
}
