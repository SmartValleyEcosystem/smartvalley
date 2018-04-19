import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {UserResponse} from './user-response';
import {EmailRequest} from './email-request';
import {EmailResponse} from './email-response';
import {UpdateUserRequest} from './update-user-request';

@Injectable()
export class UserApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getByAddressAsync(address: string): Promise<UserResponse> {
    return this.http.get<UserResponse>(this.baseApiUrl + '/users/' + address).toPromise();
  }

  public getEmailBySignatureAsync(request: EmailRequest): Promise<EmailResponse> {
    const parameters = new HttpParams()
      .append('signedText', request.signedText)
      .append('signature', request.signature);

    return this.http.get<EmailResponse>(this.baseApiUrl + '/users/' + request.address + '/email', {params: parameters}).toPromise();
  }

  public async changeEmailAsync(email: string): Promise<void> {
    await this.http.put(this.baseApiUrl + '/users/email', {email: email}).toPromise();
  }

  public async updateAsync(request: UpdateUserRequest): Promise<void> {
    await this.http.put(this.baseApiUrl + '/users', request).toPromise();
  }
}
