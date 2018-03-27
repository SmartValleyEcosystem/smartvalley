import {Component, Input} from '@angular/core';
import {CriterionWithEstimates} from '../../../services/criteria/criterionWithEstimates';
import {ScoreColorsService} from '../../../services/project/score-colors.service';


@Component({
  selector: 'app-criteria',
  templateUrl: './criteria.component.html',
  styleUrls: ['./criteria.component.css']
})
export class CriteriaComponent {
  @Input() public criteria: Array<CriterionWithEstimates>;

  constructor(public scoreColorsService: ScoreColorsService) {
  }
}
