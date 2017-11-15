import {TeamMember} from '../services/team-member';

export class Application {

  constructor() {

    this.CEO = new TeamMember();
    this.CFO = new TeamMember();
    this.CMO = new TeamMember();
    this.CTO = new TeamMember();
    this.PR = new TeamMember();
  }
  name = '';

  projectArea = '';

  probDesc = '';

  solDesc = '';

  projStat = '';

  wpLink = '';

  blockChainType = '';

  country = '';

  mvpLink = '';

  softCap = '';

  hardCap = '';

  attractInv = false;

  finModelLink = '';

  CEO: TeamMember;

  CFO: TeamMember;

  CMO: TeamMember;

  CTO: TeamMember;

  PR: TeamMember;

  authorAddress: string;
}
