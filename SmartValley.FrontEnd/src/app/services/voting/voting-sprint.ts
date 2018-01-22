import {Project} from '../project';
export interface VotingSprint {
  voteBalance: number;
  startDate: Date;
  endDate: Date;
  projects: Array<Project>;
}
