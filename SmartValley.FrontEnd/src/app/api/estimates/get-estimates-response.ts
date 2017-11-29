import {EstimateResponse} from './estimate-response';

export interface GetEstimatesResponse {
  averageScore: number;
  items: Array<EstimateResponse>;
}
