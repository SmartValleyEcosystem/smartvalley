import {AreaType} from '../scoring/area-type.enum';
import {Category} from '../../services/common/category';
import {OfferStatus} from './offer-status.enum';

export interface ScoringOfferResponse {
  scoringId: number;
  projectId: number;
  area: AreaType;
  name: string;
  scoringContractAddress: string;
  countryCode: string;
  category: Category;
  description: string;
  offerStatus: OfferStatus;
  projectExternalId: string;
  expirationTimestamp: string;
  estimatesDueDate: string;
  finalScore: number;
}
