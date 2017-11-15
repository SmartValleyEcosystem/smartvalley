import {ITeamMember} from '../services/team-member';

export class Application {

  constructor() {
    this.TeamMembers = new Array<ITeamMember>();
  }

  name = '';

  projectArea = '';

  probablyDescription = '';

  solutionDescription = '';

  projectStatus = '';

  whitePaperLink = '';

  blockChainType = '';

  country = '';

  mvpLink = '';

  softCap = '';

  hardCap = '';

  attractedInvestnemts = false;

  financeModelLink = '';

  TeamMembers: Array<ITeamMember>;

  authorAddress: string;
}
