import {AllotmentEventStatus} from './allotment-event-status';

export interface AllotmentEvent {
    id: number;
    name: string;
    status: AllotmentEventStatus;
    tokenContractAddress: string;
    startDate: string | null;
    finishDate: string;
    totalTokens: number;
    tokenTicker: string;
    tokenDecimals?: string;
}
