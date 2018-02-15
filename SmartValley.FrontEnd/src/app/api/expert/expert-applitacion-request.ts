import {ExpertiseArea} from '../scoring/expertise-area.enum';

export interface ExpertApplicationRequest {
  transactionHash: string;

  firstName: string;

  lastName: string;

  applicantAddress: string;

  birthDate: Date;

  sex: number;

  countryIsoCode: string;

  city: string;

  linkedInLink: string;

  facebookLink: string;

  description: string;

  why: string;

  documentType: number;

  documentNumber: string;

  areas: Array<ExpertiseArea>;
}
