import {ExpertUpdateRequest} from '../expert/expert-update-request';

export interface AdminExpertUpdateRequest extends ExpertUpdateRequest {
  email: string;
  address: string;
}
