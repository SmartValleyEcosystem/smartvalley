import {ScoringStatus} from '../../services/scoring-status.enum';

export interface ProjectSummaryResponse {
  id: number;
  externalId: string;
  countryCode: string;
  facebook: string;
  icoDate: Date;
  imageUrl: string;
  name: string;
  score: number;
  scoringStatus: ScoringStatus;
  stageId: number;
  category: number;
  telegram: string;
  twitter: string;
  website: string;
  whitePaperLink: string;
  authorId: number;
  scoringId: number;
  authorAddress: string;
  isApplicationSubmitted: boolean;
}
