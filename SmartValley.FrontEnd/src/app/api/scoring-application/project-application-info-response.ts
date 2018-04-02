import {AdviserResponse} from './adviser-response';
import {SocialNetworksResponse} from './social-networks-response';
import {TeamMemberResponse} from '../project/team-member-response';

export interface ProjectApplicationInfoResponse {
  name: string;
  category: number;
  stage: number;
  description: string;
  webSite: string;
  countryCode: string;
  whitePaperLink: string;
  icoDate: string;
  contactEmail: string;
  socialNetworks: SocialNetworksResponse;
  projectTeamMembers: Array<TeamMemberResponse>;
  projectAdvisers: Array<AdviserResponse>;
}
