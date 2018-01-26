import {ProjectVoteResponse} from './project-vote-response';

export interface VotingSprintResponse {
  address: string;
  startDate: Date;
  endDate: Date;
  voteBalance: number;
  maximumScore: number;
  acceptanceThreshold: number;
  number: number;
  projects: Array<ProjectVoteResponse>;
}
