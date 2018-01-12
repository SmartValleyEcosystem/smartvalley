import {Project} from '../project';
export interface Sprint {
  number: number;
  startDate: Date;
  endDate: Date;
  projects: Array<Project>;
}
