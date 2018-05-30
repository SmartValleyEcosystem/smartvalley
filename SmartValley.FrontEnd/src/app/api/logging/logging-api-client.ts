import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {LogErrorRequest} from './log-error-request';

@Injectable()
export class LoggingApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async logErrorAsync(error: LogErrorRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/logging/error', error).toPromise();
  }
}
