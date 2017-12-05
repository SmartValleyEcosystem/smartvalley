import {EstimateRequest} from './estimate-request';

export interface SubmitEstimatesRequest {
  projectId: number;

  expertAddress: string;

  expertiseArea: number;

  estimates: Array<EstimateRequest>;
}
