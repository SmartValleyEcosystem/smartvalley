import {CriterionWithEstimatesResponse} from './criterion-with-estimates-response';
import {ScoringOfferResponse} from '../scoring/scoring-offer-response';
import {ScoringAreaConslusionResponse} from '../scoring/scoring-area-conclusion-response';

export interface GetEstimatesResponse {
  score: number;
  criteria: Array<CriterionWithEstimatesResponse>;
  offers: Array<ScoringOfferResponse>;
  requiredExpertsCount: number;
  conclusions: Array<ScoringAreaConslusionResponse>;
}
