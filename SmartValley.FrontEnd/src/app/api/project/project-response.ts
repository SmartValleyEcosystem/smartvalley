import {Category} from '../../services/common/category';
import {ProjectScoringResponse} from './project-scoring-response';
import {ScoringStartTransactionStatus} from './scoring-start-transaction.status';

export interface ProjectResponse {
  id: number;
  imageUrl: string;
  name: string;
  country: string;
  category: Category;
  description: string;
  scoring?: ProjectScoringResponse;
  isApplicationSubmitted: boolean;
  scoringStartTransactionStatus: ScoringStartTransactionStatus;
  scoringStartTransactionHash: string;
  authorAddress?: string;
}
