import {AreaType} from '../scoring/area-type.enum';

export interface ExpertScoring {
  name: string;
  id: number;
  description: string;
  country: string;
  area: AreaType;
  endDate: string;
  projectImage: string;
}
