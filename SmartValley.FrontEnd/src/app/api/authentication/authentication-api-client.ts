import {BaseApiClient} from '../base-api-client';
import {HttpClient} from '@angular/common/http';
import {AuthenticationRequest} from './authentication-request';
import {AuthenticationResponse} from './authentication-response';
import {Injectable} from '@angular/core';
import {RegistrationRequest} from './registration-request';
import {ReSendEmailRequest} from './re-send-email-request';
import {ConfirmEmailRequest} from './confirm-email-request';

@Injectable()
export class AuthenticationApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public authenticateAsync(request: AuthenticationRequest): Promise<AuthenticationResponse> {
    return this.http.post<AuthenticationResponse>(this.baseApiUrl + '/auth', request).toPromise();
  }

  public async registerAsync(request: RegistrationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/auth/register', request).toPromise();
  }

  public async reSendEmailAsync(address: string): Promise<void> {
    await this.http.post(this.baseApiUrl + '/auth/resend', <ReSendEmailRequest> {address: address}).toPromise();
  }

  public async confirmEmailAsync(request: ConfirmEmailRequest): Promise<void> {
    await this.http.put(this.baseApiUrl + '/auth/confirm', request).toPromise();
  }
}
