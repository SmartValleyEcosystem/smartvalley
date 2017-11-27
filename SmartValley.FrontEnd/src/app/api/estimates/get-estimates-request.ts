import {ScoringCategory} from '../scoring/scoring-category.enum';

export interface GetEstimatesRequest {
  projectId: number;
  category: ScoringCategory;
}
