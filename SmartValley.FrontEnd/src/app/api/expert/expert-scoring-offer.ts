import {AreaType} from '../scoring/area-type.enum';

export interface ExpertScoringOffer {
  scoringId: number;
  projectId: number;
  areaId: AreaType;
  name: string;
  scoringContractAddress: string;
  country: string;
  projectArea: string;
  description: string;
  projectExternalId: string;
  expirationTimestamp: string;
  estimatesDueDate: string;
}
