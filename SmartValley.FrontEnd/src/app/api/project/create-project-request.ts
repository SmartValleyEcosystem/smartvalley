import {TeamMemberRequest} from '../application/team-member-request';
import {SocialMediaRequest} from '../application/social-media-request';

export interface CreateProjectRequest {
  name: string;
  category: number;
  stageId: number;
  projectId: string;
  description: string;
  contactEmail: string;
  whitePaperLink: string;
  countryCode: string;
  website: string;
  icoDate: Date;
  teamMembers: Array<TeamMemberRequest>;
  socialMedias: Array<SocialMediaRequest>;
}
