import {AreaResponse} from './area-response';

export interface ExpertResponse {
  id: number;
  address: string;
  email: string;
  about: string;
  isAvailable: boolean;
  isInHouse: boolean;
  firstName: string;
  secondName: string;
  areas: Array<AreaResponse>;
}
