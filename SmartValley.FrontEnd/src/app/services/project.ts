import {EnumExpertType} from './enumExpertType';

export interface Project {

  id: number;
  name: string;
  imgUrl: string;
  country: string;
  area: string;
  description: string;
  status: string;
  wpLink: string;
  score: number;
  expertType: string;
  scoringCategory: EnumExpertType;
}
