import {BalanceResponse} from './balance-response';
import {Http, Response, RequestOptions, Headers} from '@angular/http';

import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class BalanceApiClient {
  constructor(private http: HttpClient) {

  }

  async getBalance(): Promise<BalanceResponse> {
    return this.http.get<BalanceResponse>('http://localhost:5000/api/balance').toPromise();
  }

  async receiveEther() {
    await this.http.post('http://localhost:5000/api/balance', null).subscribe();
  }
}
