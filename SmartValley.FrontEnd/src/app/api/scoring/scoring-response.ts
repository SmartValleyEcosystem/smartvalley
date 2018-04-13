import {ScoringOfferResponse} from '../scoring/scoring-offer-response';
import {ScoringStatus} from '../../services/scoring-status.enum';

export interface ScoringResponse {
  id: number;
  scoringStatus: ScoringStatus;
  score: number;
  requiredExpertsCount: number;
  offers: Array<ScoringOfferResponse>;
}
