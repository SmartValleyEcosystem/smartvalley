import {EstimateResponse} from './estimate-response';
export interface QuestionWithEstimatesResponse {
  questionId: number;
  estimates: Array<EstimateResponse>;
}
