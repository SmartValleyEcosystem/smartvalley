import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {UserResponse} from './user-response';

@Injectable()
export class UserApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getByAddressAsync(address: string): Promise<UserResponse> {
    return this.http.get<UserResponse>(this.baseApiUrl + '/users/' + address).toPromise();
  }
}
