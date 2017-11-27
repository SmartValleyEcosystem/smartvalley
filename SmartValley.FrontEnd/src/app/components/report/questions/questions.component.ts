import {Component, Input} from '@angular/core';
import {Question} from '../../../services/question';
import {ProjectService} from '../../../services/project-service';


@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent {
  @Input() questions: Array<Question>
  projectService: ProjectService;

  constructor(projectService: ProjectService) {
    this.projectService = projectService;
  }
}
