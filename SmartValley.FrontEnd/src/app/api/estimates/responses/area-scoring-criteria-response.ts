import {AreaType} from '../../scoring/area-type.enum';
import {ScoringCriteriaGroupResponse} from './scoring-criteria-group-response';

export interface AreaScoringCriteriaResponse {
  area: AreaType;
  groups: Array<ScoringCriteriaGroupResponse>;
}
