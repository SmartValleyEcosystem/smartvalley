import {LogRequest} from './log-request';

export interface LogErrorRequest extends LogRequest {
    error: string;
    message: string;
    url: string;
}