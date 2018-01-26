import {ExpertiseArea} from '../api/scoring/expertise-area.enum';

export class Project {
  id: number;
  externalId: string;
  name: string;
  country: string;
  area: string;
  description: string;
  score: number;
  expertiseArea: ExpertiseArea;
  address: string;
  author: string;
  isVotedByMe: boolean;
  myVoteTokensAmount: number;
  totalTokenVote: number;
}
