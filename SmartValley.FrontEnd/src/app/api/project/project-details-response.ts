import {ScoringStatus} from '../../services/scoring-status.enum';
import {VotingStatus} from '../../services/voting-status.enum';
import {TeamMemberResponse} from './team-member-response';

export interface ProjectDetailsResponse {
  name: string;
  externalId: string;
  area: string;
  scoringContractAddress: string;
  description: string;
  status: string;
  whitePaperLink: string;
  blockChainType: string;
  country: string;
  mvpLink: string;
  softCap: string;
  hardCap: string;
  attractedInvestments: boolean;
  financeModelLink: string;
  teamMembers: Array<TeamMemberResponse>;
  authorAddress: string;
  votingAddress?: string;
  score: number;
  scoringStatus: ScoringStatus;
  votingStatus: VotingStatus;
  votingEndDate: string;
  imageUrl: string;
}
