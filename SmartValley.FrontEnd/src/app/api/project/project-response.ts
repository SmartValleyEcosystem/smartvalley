import {Category} from '../../services/common/category';
import {ProjectScoringResponse} from './project-scoring-response';

export interface ProjectResponse {
  id: number;
  name: string;
  country: string;
  category: Category;
  description: string;
  scoring?: ProjectScoringResponse;
}
