import {Component, Input} from '@angular/core';
import {Project} from '../../../services/project';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {ProjectService} from '../../../services/project-service';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent {
  @Input() public project: Project;
  @Input() public isScoring: boolean;
  projectService: ProjectService;

  constructor(private router: Router, projectService: ProjectService) {
    this.projectService = projectService;
  }

  showProject(id: number) {
    this.router.navigate(
      [Paths.Scoring + '/' + id],
      {queryParams: {category: this.project.scoringCategory}});
  }

  showReport(id: number) {
    this.router.navigate([Paths.Report + '/' + id]);
  }
}
