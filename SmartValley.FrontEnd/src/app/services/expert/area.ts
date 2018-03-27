import {AreaType} from '../../api/scoring/area-type.enum';

export interface Area {
  areaType: AreaType;
  name: string;
  maxScore: number;
}
