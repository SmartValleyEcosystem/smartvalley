import {Project} from '../project';
export interface Sprint {
  number: 1;
  startDate: Date;
  endDate: Date;
  projects: Array<Project>;
}
