import {Category} from '../../services/common/category';

export interface ScoredProject {
  id: number;
  name: string;
  address: string;
  country: string;
  category: Category;
  description: string;
  score: number;
  scoringEndDate: string;
}
