import {VotingStatus} from '../../services/voting-status.enum';
import {ScoringStatus} from '../../services/scoring-status.enum';

export interface MyProjectsItemResponse {
  author: string;
  id: number;
  name: string;
  country: string;
  address: string;
  area: string;
  description: string;
  score: number;
  scoringStatus: ScoringStatus;
  votingStatus: VotingStatus;
  votingEndDate: string;
}
