import {PendingExpertListResponse} from '../../../api/expert/pending-expert-list-response';

export interface AdminExpertResponse {
    items: PendingExpertListResponse[];
    totalCount: number;
}
