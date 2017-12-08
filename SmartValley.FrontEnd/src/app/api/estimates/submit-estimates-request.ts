import {EstimateRequest} from './estimate-request';

export interface SubmitEstimatesRequest {
  transactionHash: string;

  projectId: number;

  expertAddress: string;

  expertiseArea: number;

  estimates: Array<EstimateRequest>;
}
