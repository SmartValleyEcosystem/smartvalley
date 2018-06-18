import {AllotmentEventStatus} from '../allotment-event-status';

export interface GetAllotmentEventsRequest {
    offset: number;
    count: number;
    statuses: AllotmentEventStatus[];
}
