import {TeamMember} from './team-member';

export class Application {

  constructor() {
    this.teamMembers = [];
  }

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

  teamMembers: Array<TeamMember>;

  authorAddress: string;

  transactionHash: string;
}
