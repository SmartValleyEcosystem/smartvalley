import {AreaType} from '../scoring/area-type.enum';

export interface ScoringCriterionResponse {
  id: number;
  areaType: AreaType;
  weight: number;
}
