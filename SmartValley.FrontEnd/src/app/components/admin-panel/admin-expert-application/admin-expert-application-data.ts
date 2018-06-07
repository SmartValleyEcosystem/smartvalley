import {Area} from '../../../services/expert/area';

export interface AdminExpertApplicationData {
  id: number;
  address: string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  sex: string;
  country: string;
  city: string;
  linkedInLink: string;
  facebookLink: string;
  bitcointalkLink: string;
  description: string;
  why: string;
  documentType: string;
  documentNumber: string;
  scanUrl: string;
  photoUrl: string;
  cvUrl: string;
  areaTypes: number[];
  areas: Area[];
}
