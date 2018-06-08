import {ExpertResponse} from '../../expert/expert-response';
import {ScoringReportInAreaResponse} from './scoring-report-in-area-response';

export interface ScoringReportResponse {
  experts: Array<ExpertResponse>;
  scoringStatistics: Array<ScoringReportInAreaResponse>;
}
