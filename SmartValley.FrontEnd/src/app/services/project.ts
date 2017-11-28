import {ScoringCategory} from '../api/scoring/scoring-category.enum';

export interface Project {
  id: number;
  name: string;
  country: string;
  area: string;
  description: string;
  status: string;
  wpLink: string;
  score: number;
  expertType: string;
  scoringCategory: ScoringCategory;
  address: string;
}
