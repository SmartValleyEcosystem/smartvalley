import {Component, Input} from '@angular/core';
import {ProjectDetailsResponse} from '../../../api/project/project-details-response';
import {TeamMember} from '../../../services/team-member';
import {EnumTeamMemberType} from '../../../services/enumTeamMemberType';
import {TeamMemberResponse} from "../../../api/project/team-member-response";

@Component({
  selector: 'app-project-information',
  templateUrl: './project-information.component.html',
  styleUrls: ['./project-information.component.css']
})
export class ProjectInformationComponent {

  @Input() public projectDetails: ProjectDetailsResponse;
  @Input() public showBasicInfoCaption: boolean;

  public EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor() {
  }

  public getMembersCollection(projectDetails: ProjectDetailsResponse): Array<TeamMember> {
    const teamMembers: TeamMember[] = [];
    const memberTypeNames = Object.keys(EnumTeamMemberType).filter(key => !isNaN(Number(EnumTeamMemberType[key])));
    for (const memberType of memberTypeNames) {
      const teamMember = this.getTeamMember(projectDetails.teamMembers, memberType);
      teamMembers.push(teamMember);
    }
    return teamMembers;
  }

  private getTeamMember(teamMembers: Array<TeamMemberResponse>, memberType: string): TeamMember {
    // const teamMemberResponse = teamMembers
    //   .find(value => value.memberType === EnumTeamMemberType[memberType]);
    // return teamMemberResponse
    //   ? this.createTeamMember(teamMemberResponse)
    //   : <TeamMember>{memberType: EnumTeamMemberType[memberType]};
    return null;
  }

  private createTeamMember(teamMemberResponse: TeamMemberResponse): TeamMember {
    return <TeamMember>{
      // memberType: teamMemberResponse.memberType,
      // facebookLink: teamMemberResponse.facebookLink,
      // linkedInLink: teamMemberResponse.linkedInLink,
      fullName: teamMemberResponse.fullName
    };
  }
}
