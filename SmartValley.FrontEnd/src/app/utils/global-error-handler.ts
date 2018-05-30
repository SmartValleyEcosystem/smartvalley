import {ErrorHandler, Injectable, Injector} from '@angular/core';
import {LocationStrategy, PathLocationStrategy} from '@angular/common';
import {LoggingApiClient} from '../api/logging/logging-api-client';
import {LogErrorRequest} from '../api/logging/log-error-request';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
    constructor(private injector: Injector,
                private loggingApiClient: LoggingApiClient) { }

    handleError(error) {
        const location = this.injector.get(LocationStrategy);
        const message = error.message ? error.message : error.toString();
        const url = location instanceof PathLocationStrategy
            ? location.path() : '';
        const errorLog: LogErrorRequest = {
            error: error.stack,
            message: message,
            url: url,
            userAgent: navigator.userAgent,
            userLanguage: navigator.language
        };

        this.loggingApiClient.logErrorAsync(errorLog);
    }
}