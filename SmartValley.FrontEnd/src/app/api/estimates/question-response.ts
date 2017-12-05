import {ExpertiseArea} from '../scoring/expertise-area.enum';

export interface QuestionResponse {

  id: number;
  name: string;
  description: string;
  expertiseArea: ExpertiseArea;
  minScore: number;
  maxScore: number;
}
