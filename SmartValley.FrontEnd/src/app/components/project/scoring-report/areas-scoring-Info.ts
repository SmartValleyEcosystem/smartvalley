import {GetEstimatesResponse} from '../../../api/estimates/get-estimates-response';
import {AreaType} from '../../../api/scoring/area-type.enum';

export interface AreasScoringInfo {
  finishedExperts: number;
  totalExperts: number;
  areaName: string;
  areaType: AreaType;
  scoringInfo: GetEstimatesResponse;
}
