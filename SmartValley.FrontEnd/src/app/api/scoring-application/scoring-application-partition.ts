import {ScoringApplicationQuestion} from './scoring-application-question';

export interface ScoringApplicationPartition {
  name: string;
  order: number;
  questions: ScoringApplicationQuestion[];
}
