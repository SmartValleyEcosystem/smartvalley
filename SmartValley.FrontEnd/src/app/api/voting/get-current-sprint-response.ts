import {VotingSprintResponse} from './voting-sprint-response';

export interface GetCurrentSprintResponse {
  doesExist: boolean;
  currentSprint: VotingSprintResponse;
}
