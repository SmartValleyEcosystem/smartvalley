import {ExpertiseArea} from '../../../api/scoring/expertise-area.enum';
import {ProjectResponse} from '../../../api/project/project-response';
import {MyProjectsItemResponse} from '../../../api/project/my-projects-item-response';
import {VotingStatus} from '../../../services/voting-status.enum';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {Project} from '../../../services/project';

export class ProjectCardData {
  id: number;
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

  public static fromProject(response: Project): ProjectCardData {
    return <ProjectCardData>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      author: response.author,
      isVotedByMe: response.isVotedByMe
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
      votingEndDate: response.votingEndDate
    };
  }
}
