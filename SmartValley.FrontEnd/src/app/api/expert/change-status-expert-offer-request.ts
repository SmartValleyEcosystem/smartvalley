import {AreaType} from '../scoring/area-type.enum';

export interface ChangeStatusExpertOfferRequest {
  transactionHash: string;
  scoringId: number;
  areaId: AreaType;
}

