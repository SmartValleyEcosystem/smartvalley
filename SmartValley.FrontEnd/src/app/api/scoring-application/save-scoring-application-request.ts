import {TeamMember} from './team-member';
import {Adviser} from './adviser';
import {Answer} from './answer';

export interface SaveScoringApplicationRequest {
  projectName: string;
  projectCategory: string;
  projectStage: string;
  projectDescription: string;
  countryCode: string;
  site: string;
  whitePaper: string;
  icoDate: string;
  contactEmail: string;
  socialNetworks: {[id: string]: string};
  answers: Answer[];
  teamMembers: TeamMember[];
  advisers: Adviser[];
  articles: string;
}
