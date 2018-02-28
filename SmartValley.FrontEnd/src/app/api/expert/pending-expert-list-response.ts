import {Area} from '../../services/expert/area';

export interface PendingExpertListResponse {
  about: string;
  address: string;
  email: string;
  isAvailable: boolean;
  name: string;
  areas: Area[];
}