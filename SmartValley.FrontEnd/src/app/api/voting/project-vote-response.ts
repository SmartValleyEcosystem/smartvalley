import {VotingStatus} from '../../services/voting-status.enum';

export interface ProjectVoteResponse {
  externalId: string;
  author: string;
  id: number;
  name: string;
  country: string;
  area: string;
  description: string;
  isVotedByMe: boolean;
  myVoteTokenAmount: number;
  totalTokenAmount: number;
  votingStatus: VotingStatus;
}
