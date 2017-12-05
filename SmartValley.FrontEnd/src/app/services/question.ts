import {Estimate} from './estimate';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';

export interface Question {
  name: string;
  description: string;
  score: number;
  maxScore: number;
  minScore: number;
  comments: string;
  expertiseArea: ExpertiseArea;
  estimates: Array<Estimate>;
}
