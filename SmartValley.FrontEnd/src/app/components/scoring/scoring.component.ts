import {Component} from '@angular/core';
import {Scoring} from '../../services/scoring';
import {ScoringService} from '../../services/scoring-service';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public scorings: Array<Scoring>;

  constructor(private service: ScoringService) {
    this.scorings = service.getAll();
  }
}
