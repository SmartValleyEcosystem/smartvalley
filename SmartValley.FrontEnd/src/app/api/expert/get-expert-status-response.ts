import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';

export interface GetExpertStatusResponse {
  status: ExpertApplicationStatus;
}
