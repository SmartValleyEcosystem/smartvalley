import {AreaType} from '../scoring/area-type.enum';

export interface ExpertScoring {
  id: number;
  name: string;
  areas: Array<AreaType>;
  address: string;
}
