import {UpdateUserRequest} from '../user/update-user-request';

export interface ExpertUpdateRequest extends UpdateUserRequest {
  about: string;
  isAvailable: boolean;
}
