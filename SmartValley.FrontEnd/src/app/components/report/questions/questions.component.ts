import {Component, Input} from '@angular/core';
import {Question} from '../../../services/questions/question';
import {ProjectService} from '../../../services/project-service';


@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent {
  @Input() public questions: Array<Question>;
  public projectService: ProjectService;

  constructor(projectService: ProjectService) {
    this.projectService = projectService;
  }
}
