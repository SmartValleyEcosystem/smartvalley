import {SocialMediaRequest} from './social-media-request';

export interface TeamMemberRequest {
  fullName: string;
  role: string;
  about: string;
  socialMedias: Array<SocialMediaRequest>;
}
