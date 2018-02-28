import {Estimate} from '../estimate';
import {AreaType} from '../../api/scoring/area-type.enum';

export interface Question {
  name: string;
  description: string;
  score: number;
  maxScore: number;
  minScore: number;
  comments: string;
  areaType: AreaType;
  estimates: Array<Estimate>;
}
