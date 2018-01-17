import {EnumTeamMemberType} from '../../services/enumTeamMemberType';

export interface TeamMemberRequest {
  memberType: EnumTeamMemberType;
  fullName: string;
  facebookLink: string;
  linkedInLink: string;
}
