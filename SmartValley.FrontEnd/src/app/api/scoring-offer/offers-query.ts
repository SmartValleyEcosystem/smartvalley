import {OfferStatus} from './offer-status.enum';
import {SortDirection} from '../sort-direction.enum';
import {OffersOrderBy} from './offers-order-by';

export interface OffersQuery {
  offset: number;
  count: number;
  statuses: Array<OfferStatus>;
  orderBy?: OffersOrderBy;
  sortDirection?: SortDirection;
  expertId?: number;
  scoringId?: number;
  projectId?: number;
}
