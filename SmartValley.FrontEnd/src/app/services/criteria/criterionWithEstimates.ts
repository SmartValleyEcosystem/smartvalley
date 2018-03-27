import {Estimate} from '../estimate';
import {AreaType} from '../../api/scoring/area-type.enum';

export interface CriterionWithEstimates {
  name: string;
  description: string;
  score: number;
  comments: string;
  areaType: AreaType;
  estimates: Array<Estimate>;
}
