import {Estimate} from './estimate';
import {ScoringCategory} from '../api/scoring/scoring-category.enum';

export interface Question {
  name: string;
  description: string;
  score: number;
  maxScore: number;
  minScore: number;
  comments: string;
  expertType: ScoringCategory;
  estimates: Array<Estimate>;
  indexInCategory: number;
}
