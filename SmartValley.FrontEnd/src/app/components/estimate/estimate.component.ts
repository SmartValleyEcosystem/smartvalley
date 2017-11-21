import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Scoring} from '../../services/scoring';
import {ScoringService} from '../../services/scoring-service';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {

  public scoring: Scoring;

  constructor(private router: Router,
              private service: ScoringService) {
    const id = 0;
    this.scoring = this.service.getById(id);
  }

  ngOnInit() {
  }

}
