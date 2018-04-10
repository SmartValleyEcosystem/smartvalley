import {ScoringOfferResponse} from "../scoring/scoring-offer-response";
import {ScoringStatus} from "../../services/scoring-status.enum";

export interface ScoringResponse {
  id: number;
  scoringStatus: ScoringStatus;
  score: number;
  offers: Array<ScoringOfferResponse>;
}
