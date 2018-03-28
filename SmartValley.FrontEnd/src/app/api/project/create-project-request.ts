import {TeamMemberRequest} from "./team-member-request";

export interface CreateProjectRequest {
  name: string;
  category: number;
  stage: number;
  externalId: string;
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
  teamMembers: Array<TeamMemberRequest>;
}
