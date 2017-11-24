import {TeamMemberResponse} from '../application/team-member-response';

export interface ProjectDetailsResponse {
  name: string;
  area: string;
  projectId: string;
  description: string;
  status: string;
  whitePaperLink: string;
  blockChainType: string;
  country: string;
  mvpLink: string;
  softCap: string;
  hardCap: string;
  attractedInvestments: boolean;
  financeModelLink: string;
  teamMembers: Array<TeamMemberResponse>;
  authorAddress: string;
  score: number;
}
