import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {ApplicationApiClient} from '../../api/application/application-api.client';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent {

  public application: Application;
  public questions: Array<Question>;
  hidden: boolean;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private route: ActivatedRoute,
              private applicationApiClient: ApplicationApiClient,
              private questionService: QuestionService,
              private router: Router) {
    this.loadProjectInfo();
  }

  changeHidden() {
    this.hidden = true;
  }

  send() {
    this.router.navigate([Paths.Scoring]);
  }

  private async loadProjectInfo() {
    const id = this.route.snapshot.paramMap.get('id');
    this.questions = this.questionService.getByExpertType(2);
    this.application = await this.applicationApiClient.getByProjectIdAsync(parseInt(id));
    console.log(this.application);
  }
}
