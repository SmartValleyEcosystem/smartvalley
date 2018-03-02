import {ScoringProjectStatus} from '../../../services/scoring-project-status.enum';
import {AdminScoringProjectAreaExpertItem} from './admin-scoring-project-area-expert-item';

export interface AdminScoringProjectItem {
  projectId: string;
  title: string;
  imageUrl: string;
  startDate?: string;
  endDate?: string;
  status: string;
  statusCode: ScoringProjectStatus;
  areasExperts: Array<AdminScoringProjectAreaExpertItem>;
}
