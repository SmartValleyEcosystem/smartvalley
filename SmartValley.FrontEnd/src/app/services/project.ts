import {ExpertiseArea} from '../api/scoring/expertise-area.enum';

export interface Project {
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
}
