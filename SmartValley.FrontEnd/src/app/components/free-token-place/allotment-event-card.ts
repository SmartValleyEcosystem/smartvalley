import {AllotmentEventTimer} from './allotment-event-timer';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {ProjectResponse} from '../../api/project/project-response';

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
    timer?: AllotmentEventTimer;
    project?: ProjectResponse;
}
