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
  address: string;

  public static create(response: ProjectResponse): Project {
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

  public static createByArea(response: ProjectResponse, expertiseArea: ExpertiseArea): Project {
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
