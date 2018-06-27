import {AllotmentEventStatus} from '../allotment-event-status';
import {AllotmentEventParticipantResponse} from './allotment-event-participant-response';

export interface AllotmentEventResponse {
  id: number;
  name: string;
  status: AllotmentEventStatus;
  tokenContractAddress: string;
  eventContractAddress: string;
  projectId: number;
  startDate?: Date;
  finishDate?: Date;
  tokenDecimals: number;
  tokenTicker: string;
  participants: Array<AllotmentEventParticipantResponse>;
}
