import {ExpertiseArea} from '../../../api/scoring/expertise-area.enum';
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
  expertiseArea: ExpertiseArea;
  address: string;
  author: string;
  scoringStatus: ScoringStatus;
  votingStatus: VotingStatus;
  votingEndDate: Date;
  isVotedByMe: boolean;
  myVoteTokensAmount: number;
  projectVote: number;

  public static fromProject(response: Project): ProjectCardData {
    return <ProjectCardData>{
      id: response.id,
      externalId: response.externalId,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      author: response.author,
      myVoteTokensAmount: response.myVoteTokensAmount,
      isVotedByMe: response.isVotedByMe,
      projectVote: response.totalTokenVote,
      votingStatus: response.votingStatus
    };
  }

  public static fromProjectResponse(response: ProjectResponse,
                                    expertiseArea: ExpertiseArea = ExpertiseArea.HR): ProjectCardData {
    return <ProjectCardData>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      author: response.author,
      expertiseArea: expertiseArea,
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
