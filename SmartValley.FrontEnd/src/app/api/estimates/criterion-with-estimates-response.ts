import {EstimateResponse} from './estimate-response';
export interface CriterionWithEstimatesResponse {
  scoringCriterionId: number;
  estimates: Array<EstimateResponse>;
}
