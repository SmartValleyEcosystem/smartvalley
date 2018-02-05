import {HttpClient, HttpParams} from '@angular/common/http';
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

  public async addAsync(address: string, transactionHash: string): Promise<void> {
    await this.http.post(this.baseApiUrl + '/admin', <AdminRequest> {
      address: address,
      transactionHash: transactionHash
    }).toPromise();
  }

  public async deleteAsync(address: string, transactionHash: string): Promise<void> {
    const params = new HttpParams().set('address', address);
    params.set('transactionHash', transactionHash);
    await this.http.delete(this.baseApiUrl + '/admin/delete', {params}).toPromise();
  }

  public getAllAsync(): Promise<CollectionResponse<AdminResponse>> {
    return this.http.get<CollectionResponse<AdminResponse>>(this.baseApiUrl + '/admin').toPromise();
  }
}
