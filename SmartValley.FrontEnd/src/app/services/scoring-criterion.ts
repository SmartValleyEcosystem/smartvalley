import {AreaType} from '../api/scoring/area-type.enum';

export interface ScoringCriterion {
  id: number;
  area: AreaType;
  description: string;
}
