import {EnumTeamMemberType} from './enumTeamMemberType';

export interface TeamMember {
  memberType: EnumTeamMemberType;
  fullName: string;
  facebookLink: string;
  linkedInLink: string;
}
