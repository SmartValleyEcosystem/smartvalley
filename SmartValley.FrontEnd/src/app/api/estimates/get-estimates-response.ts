import {CriterionWithEstimatesResponse} from './criterion-with-estimates-response';
export interface GetEstimatesResponse {
  score: number;
  criteria: Array<CriterionWithEstimatesResponse>;
}
