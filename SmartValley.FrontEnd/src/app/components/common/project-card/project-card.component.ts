import {Component, Input, OnInit} from '@angular/core';
import {Project} from '../../../services/project';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent {
  @Input() public project: Project;
  @Input() public isScoring: boolean;

  constructor(private router: Router) { }

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
    return 'low_rate';
  }

  showProject(id: number) {
    this.router.navigate([Paths.Scoring + '/' + id]);
  }

  showReport(id: number) {
    console.log(id);

    this.router.navigate([Paths.Report + '/' + id]);
  }

}
