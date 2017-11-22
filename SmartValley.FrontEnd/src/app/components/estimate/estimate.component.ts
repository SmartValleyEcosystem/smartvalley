import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Application} from '../../services/application';
import {ApplicationService} from '../../services/application-service';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {Paths} from '../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {

  public application: Application;
  public questions: Array<Question>;
  hidden: boolean;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private route: ActivatedRoute,
              private applicationService: ApplicationService,
              private questionService: QuestionService,
              private router: Router) {
  }

  changeHidden() {
    this.hidden = true;
  }

  send() {
    this.router.navigate([Paths.Scoring]);
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.application = this.applicationService.getById(id);
    this.questions = this.questionService.getByExpertType(2);
  }
}
