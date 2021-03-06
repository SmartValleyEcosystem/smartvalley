import {SexEnum} from '../../components/register-expert/sex.enum';
import {DocumentEnum} from '../../components/register-expert/document.enum';

export interface ExpertApplicationResponse {
  id: number;
  address: string;
  firstName: string;
  lastName: string;
  birthDate: string;
  sex: SexEnum;
  countryIsoCode: string;
  city: string;
  linkedInLink: string;
  facebookLink: string;
  bitcointalkLink: string;
  description: string;
  why: string;
  documentType: DocumentEnum;
  documentNumber: string;
  scanUrl: string;
  photoUrl: string;
  cvUrl: string;
  areas: number[];
}

