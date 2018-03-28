import {CategoryEnum} from '../../services/common/category.enum';

export interface ProjectResponse {
  id: number;
  name: string;
  address: string;
  country: string;
  category: CategoryEnum;
  description: string;
  score: number;
  scoringEndDate: string;
}
