import {VotingSprintResponse} from './voting-sprint-response';

export interface GetSprintResponse {
  doesExist: boolean;
  sprint: VotingSprintResponse;
}
