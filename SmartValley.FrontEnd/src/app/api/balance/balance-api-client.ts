import {BalanceResponse} from './balance-response';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';

@Injectable()
export class BalanceApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  async getBalance(): Promise<BalanceResponse> {
    return this.http.get<BalanceResponse>(this.baseApiUrl + '/balance').toPromise();
  }

  async receiveEther() {
    await this.http.post(this.baseApiUrl + '/balance', null).subscribe();
  }
}
