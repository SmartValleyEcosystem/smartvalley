import {ScoringCriterionResponse} from './scoring-criterion-response';

export interface ScoringCriteriaGroupResponse {
  key: string;
  order: number;
  criteria: Array<ScoringCriterionResponse>;
}
