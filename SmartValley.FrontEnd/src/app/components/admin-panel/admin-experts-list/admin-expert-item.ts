import {Area} from '../../../services/expert/area';

export interface AdminExpertItem {
    name: string;
    about: string;
    address: string;
    email: string;
    isAvailable: boolean;
    areas: Area[];
}
