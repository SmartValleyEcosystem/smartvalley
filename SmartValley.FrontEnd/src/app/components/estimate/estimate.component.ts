import {Component} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {EnumExpertType} from '../../services/enumExpertType';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {EstimateRequest} from '../../api/estimates/estimate-request';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent {
  hidden: boolean;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;
  expertType: EnumExpertType;
  projectId: number;

  public application: ProjectDetailsResponse;
  public questions: Array<Question>;

  constructor(private route: ActivatedRoute,
              private projectApiClient: ProjectApiClient,
              private questionService: QuestionService,
              private router: Router,
              private estimatesApiClient: EstimatesApiClient,
              private authenticationService: AuthenticationService) {
    this.loadProjectInfo();
  }

  changeHidden() {
    this.hidden = true;
  }

  async send() {
    await this.submitAsync();
    await this.router.navigate([Paths.Scoring]);
  }

  private async submitAsync(): Promise<void> {
    const estimates = this.getEstimates();
    const submitEstimatesRequest = <SubmitEstimatesRequest>{
      projectId: this.projectId,
      category: this.expertType,
      expertAddress: this.authenticationService.getCurrentUser().account,
      estimates: estimates
    };

    await this.estimatesApiClient.submitEstimatesAsync(submitEstimatesRequest);
  }

  private getEstimates(): Array<EstimateRequest> {
    const estimates: Array<EstimateRequest> = [];

    for (const question of this.questions) {
      estimates.push(<EstimateRequest>{
        questionIndex: question.indexInCategory,
        score: question.score,
        comment: question.comments
      });
    }

    return estimates;
  }

  private async loadProjectInfo() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.expertType = EnumExpertType.Lawyer; // TODO
    this.questions = this.questionService.getByExpertType(this.expertType);
    this.application = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
  }
}
