import {AreaType} from '../scoring/area-type.enum';

export interface AreaResponse {
  id: AreaType;
  name: string;
  maxScore: number;
}
