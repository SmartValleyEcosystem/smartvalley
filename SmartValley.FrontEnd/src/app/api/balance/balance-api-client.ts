import {BalanceResponse} from './balance-response';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';

@Injectable()
export class BalanceApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getBalanceAsync(): Promise<BalanceResponse> {
    return this.http.get<BalanceResponse>(this.baseApiUrl + '/balance').toPromise();
  }

  async receiveEtherAsync(): Promise<void> {
    await this.http.post(this.baseApiUrl + '/balance', null).subscribe();
  }
}
