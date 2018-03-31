import {TeamMemberResponse} from './team-member-response';

export interface ProjectAboutResponse {
  projectId: number;
  description: string;
  facebook: string;
  linkedin: string;
  bitcoinTalk: string;
  medium: string;
  reddit: string;
  telegram: string;
  twitter: string;
  TeamMembers: TeamMemberResponse[];
}
