import {Component, Input, OnInit} from '@angular/core';
import {Question} from '../../../services/question';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit {
  @Input() questions: Array<Question>

  constructor() { }

  ngOnInit() {
  }

  colorOfEstimateScore(rate: number): string {
    if (rate == null) {
      return '';
    }
    if (rate > 4) {
      return 'high_rate';
    }
    if (rate > 2) {
      return 'medium_rate';
    }
    if (rate >= 0) {
      return 'low_rate';
    }
    return 'progress_rate';
  }

}
