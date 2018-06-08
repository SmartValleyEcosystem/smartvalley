import {ScoringReportInAreaResponse} from '../../../api/estimates/responses/scoring-report-in-area-response';
import {AreaType} from '../../../api/scoring/area-type.enum';

export interface AreasScoringInfo {
  finishedExperts: number;
  totalExperts: number;
  areaName: string;
  areaType: AreaType;
  scoringInfo: ScoringReportInAreaResponse;
}
