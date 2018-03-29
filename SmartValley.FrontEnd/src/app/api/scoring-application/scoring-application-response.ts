import {ScoringApplicationPartition} from './scoring-application-partition';

export interface ScoringApplicationResponse {
  created: string;
  saved: string;
  partitions: ScoringApplicationPartition[];
}
