import {AreaType} from '../scoring/area-type.enum';

export interface QuestionResponse {

  id: number;
  name: string;
  description: string;
  areaType: AreaType;
  minScore: number;
  maxScore: number;
}
