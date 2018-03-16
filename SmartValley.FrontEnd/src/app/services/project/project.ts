import {AreaType} from '../../api/scoring/area-type.enum';
import {VotingStatus} from '../voting-status.enum';

export class Project {
  id: number;
  externalId: string;
  name: string;
  country: string;
  area: string;
  description: string;
  score: number;
  areaType: AreaType;
  address: string;
  author: string;
  isVotedByMe: boolean;
  myVoteTokensAmount: number;
  totalTokenVote: number;
  votingStatus: VotingStatus;
}
