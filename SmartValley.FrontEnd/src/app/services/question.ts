import {EnumExpertType} from './enumExpertType';

export interface Question {
  name: string;
  description: string;
  score: number;
  maxScore: number;
  comments: string;
  expertType: EnumExpertType;
}
