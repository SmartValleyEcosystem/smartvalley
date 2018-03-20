import {ProjectCategoryEnum} from '../../services/project/project-category.enum';

export interface ScoredProject {
  id: number;
  name: string;
  address: string;
  country: string;
  category: ProjectCategoryEnum;
  description: string;
  score: number;
  scoringEndDate: string;
}
