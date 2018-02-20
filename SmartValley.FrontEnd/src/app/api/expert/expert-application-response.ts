import {SexEnum} from '../../components/register-expert/sex.enum';
import {DocumentEnum} from '../../components/register-expert/document.enum';

export interface ExpertApplicationResponse {
  id: number;
  firstName: string;
  lastName: string;
  birthDate: string;
  sex: SexEnum;
  countryIsoCode: string;
  city: string;
  linkedInLink: string;
  facebookLink: string;
  description: string;
  why: string;
  documentType: DocumentEnum;
  documentNumber: string;
  scanName: string;
  photoName: string;
  cvName: string;
  areas: number[];
}

