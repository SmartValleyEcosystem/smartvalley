import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {AllotmentEventTimer} from './allotment-event-timer';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';

export interface AllotmentEventCard {
    id?: number;
    name: string;
    status: AllotmentEventStatus;
    project?: ProjectSummaryResponse;
    tokenContractAddress: string;
    eventContractAddress?: string;
    startDate?: string | null;
    tokenDecimals: number;
    tokenTicker: string;
    projectId?: number;
    finishDate?: Date;
    timer?: AllotmentEventTimer;
}
