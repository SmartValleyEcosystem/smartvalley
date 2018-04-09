import {ScoringCriterion} from './scoring-criterion';

export interface ScoringCriteriaGroup {
  name: string;
  order: number;
  criteria: Array<ScoringCriterion>;
}
