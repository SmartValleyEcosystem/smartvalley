import {Area} from '../../../services/expert/area';

export interface AdminExpertApplicationData {
  id: number;
  firstName: string;
  lastName: string;
  birthDate: Date;
  sex: string;
  country: string;
  city: string;
  linkedInLink: string;
  facebookLink: string;
  description: string;
  why: string;
  documentType: string;
  documentNumber: string;
  scanName: string;
  photoName: string;
  cvName: string;
  areaTypes: number[];
  areas: Area[];
}
