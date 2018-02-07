import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {AuthenticationRequest} from './authentication-request';
import {AuthenticationResponse} from './authentication-response';
import {Injectable} from '@angular/core';
import {RegistrationRequest} from './registration-request';

@Injectable()
export class AuthenticationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async authenticateAsync(request: AuthenticationRequest): Promise<AuthenticationResponse> {
    return await this.http.post<AuthenticationResponse>(this.baseApiUrl + '/auth', request).toPromise();
  }

  public async registerAsync(request: RegistrationRequest): Promise<void> {
     await this.http.post(this.baseApiUrl + '/auth/register', request).toPromise();
  }
}
