import {Component, Input, OnInit} from '@angular/core';
import {Scoring} from '../../../services/scoring';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent implements OnInit {
  @Input() public project: Scoring;

  constructor() { }

  ngOnInit() {
  }

}
