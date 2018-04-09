import {ScoringCriterionOption} from './scoring-criterion-option';

export interface ScoringCriterion {
  id: number;
  name: string;
  weight: number;
  options: Array<ScoringCriterionOption>;
}
