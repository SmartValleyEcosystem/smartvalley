import {UpdateUserRequest} from '../user/update-user-request';

export interface AdminExpertUpdateRequest extends UpdateUserRequest {
  email: string;
  address: string;
  isInHouse: boolean;
  isAvailable: boolean;
}
