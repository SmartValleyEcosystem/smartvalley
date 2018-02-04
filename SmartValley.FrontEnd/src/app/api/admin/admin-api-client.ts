import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {AdminRequest} from './admin-request';
import {AdminResponse} from './admin-response';
import {CollectionResponse} from '../collection-response';

@Injectable()
export class AdminApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async addAdminAsync(address: string, transactionHash: string): Promise<void> {
    await this.http.post(this.baseApiUrl + '/admin', <AdminRequest> {
      address: address,
      transactionHash: transactionHash
    }).toPromise();
  }

  public async deleteAdminAsync(address: string, transactionHash: string): Promise<void> {
    await this.http.post(this.baseApiUrl + '/admin/delete', <AdminRequest> {
      address: address,
      transactionHash: transactionHash
    }).toPromise();
  }

  public getAllAdminsAsync(): Promise<CollectionResponse<AdminResponse>> {
    return this.http.get<CollectionResponse<AdminResponse>>(this.baseApiUrl + '/admin').toPromise();
  }
}
