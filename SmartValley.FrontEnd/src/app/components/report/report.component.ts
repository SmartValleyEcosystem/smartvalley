import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ProjectService} from '../../services/project-service';
import {Application} from '../../services/application';
import {ActivatedRoute} from '@angular/router';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  project: Project;
  application: Application;
  public questions: Array<Question>;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private projectService: ProjectService,
              private applicationApiClient: ApplicationApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute) {
    this.questions = this.questionService.getByExpertType(2);
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = parseInt(idParam, 0);
    this.project = projectService.getById(id);
    this.loadApplication(id);
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

  private async loadApplication(id: number) {
    this.application = await this.applicationApiClient.getByProjectIdAsync(id);
    console.log(this.application);
  }
}
