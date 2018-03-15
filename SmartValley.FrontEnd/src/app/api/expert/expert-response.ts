import {Area} from '../../services/expert/area';

export interface ExpertResponse {
  about: string;
  address: string;
  email: string;
  isAvailable: boolean;
  name: string;
  areas: any;
}
