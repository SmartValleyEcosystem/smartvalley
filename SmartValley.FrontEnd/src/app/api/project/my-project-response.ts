import {TeamMemberResponse} from './team-member-response';

export interface MyProjectResponse {
  id: number;
  name: string;
  externalId: string;
  category: number;
  stage: number;
  description: string;
  contactEmail: string;
  whitePaperLink: string;
  countryCode: string;
  website: string;
  icoDate: Date;
  facebook: string;
  bitcointalk: string;
  medium: string;
  reddit: string;
  telegram: string;
  twitter: string;
  github: string;
  linkedin: string;
  teamMembers: Array<TeamMemberResponse>;
}
