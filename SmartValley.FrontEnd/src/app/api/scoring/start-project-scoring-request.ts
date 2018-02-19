import {AreaRequest} from './area-request';

export interface StartProjectScoringRequest {
  projectExternalId: string;
  areas: Array<AreaRequest>;
  transactionHash: string;
}
