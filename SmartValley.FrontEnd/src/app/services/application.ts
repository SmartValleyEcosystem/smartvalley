import {TeamMember} from '../services/team-member';

export class Application {

  constructor() {
    this.TeamMembers = new Array<TeamMember>();
  }

  name: string;

  projectArea: string;

  probablyDescription: string;

  solutionDescription: string;

  projectStatus: string;

  whitePaperLink: string;

  blockChainType: string;

  country: string;

  mvpLink: string;

  softCap: string;

  hardCap: string;

  attractedInvestnemts = false;

  financeModelLink: string;

  TeamMembers: Array<TeamMember>;

  authorAddress: string;
}
