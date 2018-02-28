import {ScoringProjectStatus} from '../../services/scoring-project-status.enum';
import {AreaExpertResponse} from './area-expert-response';

export interface ScoringProjectResponse {
  projectId: string;
  name: string;
  address: string;
  startDate: Date;
  endDate: Date;
  status: ScoringProjectStatus;
  areasExperts: Array<AreaExpertResponse>;
}
