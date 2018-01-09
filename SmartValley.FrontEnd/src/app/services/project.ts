import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {ProjectResponse} from '../api/project/project-response';

export class Project {
  id: number;
  name: string;
  country: string;
  area: string;
  description: string;
  status: string;
  wpLink: string;
  score: number;
  expertType: string;
  expertiseArea: ExpertiseArea;
  address: string

  public static createProject(response: ProjectResponse): Project {
    return <Project>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address
    };
  }

  public static createProjectByArea(response: ProjectResponse, expertiseArea: ExpertiseArea = ExpertiseArea.HR): Project {
    return <Project>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      expertiseArea: expertiseArea
    };
  }
}
