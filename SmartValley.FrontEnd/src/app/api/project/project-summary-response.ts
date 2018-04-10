import {ScoringResponse} from "../scoring/scoring-response";

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
  scoring: ScoringResponse;
}
