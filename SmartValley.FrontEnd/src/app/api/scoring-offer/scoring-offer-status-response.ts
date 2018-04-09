import {OfferStatus} from './offer-status.enum';

export interface ScoringOfferStatusResponse {
  exists: boolean;
  status: OfferStatus;
}
