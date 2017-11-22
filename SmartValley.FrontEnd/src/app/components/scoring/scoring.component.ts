import {Component} from '@angular/core';
import {Scoring} from '../../services/scoring';
import {Project} from '../../services/project';
import {ScoringService} from '../../services/scoring-service';
import {ProjectService} from '../../services/project-service';
import {Paths} from '../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public scorings: Array<Scoring>;
  public myProjects: Array<Project>;

  constructor(private scoringService: ScoringService,
              private projectService: ProjectService,
              private router: Router) {
    this.scorings = scoringService.getAll();
    this.myProjects = projectService.getAll();
  }

  showProject(id: number) {
    this.router.navigate([Paths.Scoring + '/' + id]);
  }
}
