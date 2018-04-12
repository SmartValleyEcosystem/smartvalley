import {AreaRequest} from './area-request';

export interface StartProjectScoringRequest {
  projectId: number;
  areas: Array<AreaRequest>;
  transactionHash: string;
}
