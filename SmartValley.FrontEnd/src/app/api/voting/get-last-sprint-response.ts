import {VotingSprintResponse} from './voting-sprint-response';

export interface GetLastSprintResponse {
  doesExist: boolean;
  lastSprint: VotingSprintResponse;
}
