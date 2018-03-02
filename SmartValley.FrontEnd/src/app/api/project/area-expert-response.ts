import {AreaType} from '../scoring/area-type.enum';

export interface AreaExpertResponse {
  areaType: AreaType;
  acceptedCount: number;
  requiredCount: number;
}
