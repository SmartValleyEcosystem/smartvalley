import {GetEstimatesResponse} from '../../../api/estimates/get-estimates-response';

export interface AreasScoringInfo {
  areaType: number;
  areaName: string;
  scoringInfo: GetEstimatesResponse;
}
