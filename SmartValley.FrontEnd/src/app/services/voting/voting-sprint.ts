import {Project} from '../project';

export interface VotingSprint {
  address: string;
  voteBalance: number;
  startDate: Date;
  endDate: Date;
  number: number;
  maximumScore: number;
  acceptanceThreshold: number;
  projects: Array<Project>;
}
