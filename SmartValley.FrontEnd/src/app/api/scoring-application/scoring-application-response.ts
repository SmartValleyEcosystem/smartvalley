import {ScoringApplicationPartition} from './scoring-application-partition';
import {ProjectApplicationInfoResponse} from './project-application-info-response';

export interface ScoringApplicationResponse {
  projectInfo: ProjectApplicationInfoResponse;
  created: string;
  saved: string;
  partitions: ScoringApplicationPartition[];
  isSubmitted: boolean;
}
