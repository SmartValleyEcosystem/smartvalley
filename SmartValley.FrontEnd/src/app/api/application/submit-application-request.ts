import {TeamMemberRequest} from './team-member-request';

export interface SubmitApplicationRequest {
  name: string;
  projectArea: string;
  projectId: string;
  description: string;
  projectStatus: string;
  whitePaperLink: string;
  blockChainType: string;
  country: string;
  mvpLink: string;
  softCap: string;
  hardCap: string;
  attractedInvestments: boolean;
  financeModelLink: string;
  teamMembers: Array<TeamMemberRequest>;
  authorAddress: string;
}
