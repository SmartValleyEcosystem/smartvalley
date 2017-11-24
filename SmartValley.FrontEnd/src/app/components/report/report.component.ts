import {Component, OnInit} from '@angular/core';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ActivatedRoute} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  selectedIndex = 0;
  application: Application;
  public questions: Array<Question>;
  report: ProjectDetailsResponse;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private projectApiClient: ProjectApiClient,
              private applicationApiClient: ApplicationApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute) {
    this.questions = this.questionService.getByExpertType(2);
    this.loadApplication();
  }

  clickMe() {
    this.selectedIndex = 1;
  }

  colorOfProjectRate(rate: number): string {
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

  colorOfEstimateScore(rate: number): string {
    if (rate == null) {
      return '';
    }
    if (rate > 4) {
      return 'high_rate';
    }
    if (rate > 2) {
      return 'medium_rate';
    }
    if (rate >= 0) {
      return 'low_rate';
    }
    return 'progress_rate';
  }

  private async loadData() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.report = await this.projectApiClient.getDetailsByIdAsync(id);
  }
}
