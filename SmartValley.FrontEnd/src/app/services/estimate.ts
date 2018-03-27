import {Score} from './score.enum';

export interface Estimate {
  scoringCriterionId: number;
  score: Score;
  comments: string;
}
