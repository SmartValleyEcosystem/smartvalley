import {TeamMember} from './team-member';

export class Application {

  constructor() {
    this.teamMembers = [];
  }

  name: string;

  projectArea: string;

  projectId: string;

  probablyDescription: string;

  solutionDescription: string;

  projectStatus: string;

  whitePaperLink: string;

  blockChainType: string;

  country: string;

  mvpLink: string;

  softCap: string;

  hardCap: string;

  attractedInvestments = false;

  financeModelLink: string;

  teamMembers: Array<TeamMember>;

  authorAddress: string;

  transactionHash: string;
}
