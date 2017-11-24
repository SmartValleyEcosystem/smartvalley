import {EnumExpertType} from './enumExpertType';
import {Estimate} from './estimate';

export interface Question {
  name: string;
  description: string;
  score: number;
  maxScore: number;
  comments: string;
  expertType: EnumExpertType;
  estimates: Array<Estimate>;
}
