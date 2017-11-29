import {BalanceResponse} from './balance-response';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {ReceiveEtherResponse} from './receive-ether-response';

@Injectable()
export class BalanceApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getBalanceAsync(): Promise<BalanceResponse> {
    return this.http.get<BalanceResponse>(this.baseApiUrl + '/balance').toPromise();
  }

  async receiveEtherAsync(): Promise<ReceiveEtherResponse> {
    return this.http.post<ReceiveEtherResponse>(this.baseApiUrl + '/balance', null).toPromise();
  }
}
