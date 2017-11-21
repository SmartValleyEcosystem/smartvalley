import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Scoring} from '../../services/scoring';
import {ScoringService} from '../../services/scoring-service';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {

  public scoring: Scoring;

  constructor(private route: ActivatedRoute,
              private service: ScoringService) {
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.scoring = this.service.getById(parseInt(id, 0));
    console.log(this.scoring);
  }
}
