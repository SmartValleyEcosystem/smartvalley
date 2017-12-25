import {Component, Input, OnInit} from '@angular/core';
import {ProjectDetailsResponse} from '../../../api/project/project-details-response';
import {TeamMember} from '../../../services/team-member';
import {EnumTeamMemberType} from '../../../services/enumTeamMemberType';
import {TeamMemberResponse} from '../../../api/application/team-member-response';

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
      const teamMemberResponse = projectDetails.teamMembers.find(value => value.memberType === EnumTeamMemberType[memberType])
        || <TeamMemberResponse>{memberType: EnumTeamMemberType[memberType], fullName: '\u2014'};

      teamMembers.push(this.createTeamMember(teamMemberResponse));
    }
    return teamMembers;
  }

  private createTeamMember(teamMemberResponse: TeamMemberResponse): TeamMember {
    return <TeamMember>{
      memberType: teamMemberResponse.memberType,
      facebookLink: teamMemberResponse.facebookLink,
      linkedInLink: teamMemberResponse.linkedInLink,
      fullName: teamMemberResponse.fullName
    };
  }
}
