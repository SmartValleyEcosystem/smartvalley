import {AllotmentEventStatus} from '../allotment-event-status';

export interface AllotmentEventResponse {
  id: number;
  name: string;
  isUpdating: boolean;
  status: AllotmentEventStatus;
  tokenContractAddress: string;
  eventContractAddress: string;
  projectId: number;
  startDate: string | null;
  finishDate: string | null;
  tokenDecimals: number;
  tokenTicker: string;
}
