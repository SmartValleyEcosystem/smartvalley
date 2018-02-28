import {AreaType} from '../../../api/scoring/area-type.enum';
import {ProjectResponse} from '../../../api/project/project-response';
import {MyProjectsItemResponse} from '../../../api/project/my-projects-item-response';
import {VotingStatus} from '../../../services/voting-status.enum';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {Project} from '../../../services/project';
import * as moment from 'moment';
import {isNullOrUndefined} from 'util';

export class ProjectCardData {
  id: number;
  externalId: string;
  name: string;
  country: string;
  area: string;
  description: string;
  score: number;
  areaType: AreaType;
  address: string;
  author: string;
  scoringStatus: ScoringStatus;
  votingStatus: VotingStatus;
  votingEndDate: Date;
  isVotedByMe: boolean;
  myVoteTokensAmount: number;
  projectVote: number;

  public static fromProject(project: Project): ProjectCardData {
    return <ProjectCardData>{
      id: project.id,
      externalId: project.externalId,
      name: project.name,
      area: project.area,
      country: project.country,
      score: project.score,
      description: project.description,
      address: project.address,
      author: project.author,
      myVoteTokensAmount: project.myVoteTokensAmount,
      isVotedByMe: project.isVotedByMe,
      projectVote: project.totalTokenVote,
      votingStatus: project.votingStatus
    };
  }

  public static fromProjectResponse(response: ProjectResponse,
                                    areaType: AreaType = AreaType.HR): ProjectCardData {
    return <ProjectCardData>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      author: response.author,
      areaType: areaType,
      scoringStatus: response.score ? ScoringStatus.Finished : ScoringStatus.InProgress
    };
  }

  public static fromMyProjectsItemResponse(response: MyProjectsItemResponse): ProjectCardData {
    return <ProjectCardData>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      author: response.author,
      scoringStatus: response.scoringStatus,
      votingStatus: response.votingStatus,
      votingEndDate: !isNullOrUndefined(response.votingEndDate) ? moment(response.votingEndDate).toDate() : null
    };
  }
}
