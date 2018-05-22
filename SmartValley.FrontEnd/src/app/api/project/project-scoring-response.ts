import {Category} from '../../services/common/category';

export interface ProjectScoringResponse {
  id: number;
  name: string;
  contractAddress: string;
  country: string;
  category: Category;
  description: string;
  score: number;
  creationDate: string;
  offersDueDate: string;
  scoringStartDate: string;
  scoringEndDate: string;
  estimatesDueDate: string;
  scoringStatus: number;
  expertsCount: number;
}
