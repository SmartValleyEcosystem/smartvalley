import {EstimateRequest} from './estimate-request';

export interface SubmitEstimatesRequest {
  projectId: number;

  expertAddress: string;

  category: number;

  estimates: Array<EstimateRequest>;
}
