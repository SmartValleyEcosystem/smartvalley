import {ScoringResponse} from '../scoring/scoring-response';
import {ScoringStartTransactionStatus} from './scoring-start-transaction.status';

export interface ProjectSummaryResponse {
  id: number;
  externalId: string;
  countryCode: string;
  facebook: string;
  icoDate: Date;
  imageUrl: string;
  name: string;
  stageId: number;
  category: number;
  telegram: string;
  twitter: string;
  website: string;
  whitePaperLink: string;
  authorId: number;
  authorAddress: string;
  isApplicationSubmitted: boolean;
  isPrivate: boolean;
  scoring: ScoringResponse;
  scoringStartTransactionStatus: ScoringStartTransactionStatus;
  scoringStartTransactionHash: string;
}
