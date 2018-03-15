import {ExpertResponse} from '../../../api/expert/expert-response';

export interface AdminExpertResponse {
    items: ExpertResponse[];
    totalCount: number;
}
