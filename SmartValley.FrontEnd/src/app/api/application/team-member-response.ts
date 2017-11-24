import {EnumTeamMemberType} from '../../services/enumTeamMemberType';

export interface TeamMemberResponse {
  memberType: EnumTeamMemberType;
  fullName: string;
  facebookLink: string;
  linkedInLink: string;
}
