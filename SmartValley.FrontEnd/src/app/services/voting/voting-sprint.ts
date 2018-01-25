import {Project} from '../project';

export interface VotingSprint {
  address: string;
  voteBalance: number;
  startDate: Date;
  endDate: Date;
  number: number;
  projects: Array<Project>;
}
