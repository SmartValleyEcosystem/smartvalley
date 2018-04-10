import {AreaType} from "./area-type.enum";
import {OfferStatus} from "../scoring-offer/offer-status.enum";

export interface ScoringOfferResponse {
  area: AreaType;
  status: OfferStatus;
}
