import {AreaType} from '../scoring/area-type.enum';

export interface SubmitEstimatesRequest {
  transactionHash: string;
  projectId: number;
  areaType: AreaType;
}
