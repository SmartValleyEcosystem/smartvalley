import {TeamMemberResponse} from '../application/team-member-response';
import {ScoringStatus} from '../../services/scoring-status.enum';
import {VotingStatus} from '../../services/voting-status.enum';

export interface ProjectDetailsResponse {
  name: string;
  externalId: string;
  area: string;
  projectAddress: string;
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
  votingAddress: string;
  score: number;
  scoringStatus: ScoringStatus;
  votingStatus: VotingStatus;
  votingEndDate: string;
}
