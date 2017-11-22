import {Component} from '@angular/core';
import {Scoring} from '../../services/scoring';
import {ScoringService} from '../../services/scoring-service';
import {Paths} from '../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public scorings: Array<Scoring>;

  constructor(private service: ScoringService,
              private router: Router) {
    this.scorings = service.getAll();
  }

  showProject(id: number) {
    this.router.navigate([Paths.Scoring + '/' + id]);
  }
}
