import {ExpertiseArea} from '../api/scoring/expertise-area.enum';

export class Project {
  id: number;
  name: string;
  country: string;
  area: string;
  description: string;
  score: number;
  expertiseArea: ExpertiseArea;
  address: string;
  author: string;
  isVotedByMe: boolean;
}
