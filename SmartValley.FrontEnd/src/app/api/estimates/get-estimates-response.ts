import {QuestionWithEstimatesResponse} from './question-with-estimates-response';
export interface GetEstimatesResponse {
  averageScore: number;
  questions: Array<QuestionWithEstimatesResponse>;
}
