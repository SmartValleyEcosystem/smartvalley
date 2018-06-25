import {AllotmentEventTimer} from './allotment-event-timer';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {ProjectResponse} from '../../api/project/project-response';
import {AllotmentEventParticipantResponse} from '../../api/allotment-events/responses/allotment-event-participant-response';

export interface AllotmentEventCard {
  id?: number;
  name: string;
  status: AllotmentEventStatus;
  tokenContractAddress: string;
  eventContractAddress?: string;
  startDate?: Date;
  tokenDecimals: number;
  tokenTicker: string;
  projectId?: number;
  finishDate?: Date;
  participants: Array<AllotmentEventParticipantResponse>;
  timer?: AllotmentEventTimer;
  project?: ProjectResponse;
}
