import {AllotmentEventStatus} from '../allotment-event-status';

export interface AllotmentEventResponse {
  id: number;
  name: string;
  projectId?: number;
  status: AllotmentEventStatus;
  tokenContractAddress: string;
  eventContractAddress: string;
  projectId: number;
  startDate: string | null;
  finishDate: string | null;
  tokenDecimals: number;
  tokenTicker: string;
}
