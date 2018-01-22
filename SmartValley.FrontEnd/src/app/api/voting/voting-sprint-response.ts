import {ProjectVoteResponse} from './project-vote-response';

export interface VotingSprintResponse {
  startDate: Date;
  endDate: Date;
  voteBalance: number;
  projects: Array<ProjectVoteResponse>;
}
