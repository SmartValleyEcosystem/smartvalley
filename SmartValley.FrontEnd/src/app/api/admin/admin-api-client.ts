import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {AdminRequest} from './admin-request';
import {AdminResponse} from './admin-response';
import {CollectionResponse} from '../collection-response';
import {AdminExpertUpdateRequest} from './admin-expert-update-request';
import {AdminSetAvailabilityRequest} from './admin-set-availability-request';
import {AdminExpertUpdateAreasRequest} from './admin-expert-update-areas-request';
import {AdminUserUpdateRequest} from './admin-user-update-request';
import {UserResponse} from '../user/user-response';

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

  public updateExpertAsync(updateRequest: AdminExpertUpdateRequest) {
    return this.http.put(this.baseApiUrl + '/admin/experts', updateRequest).toPromise();
  }

  public updateExpertAreasAsync(request: AdminExpertUpdateAreasRequest) {
    return this.http.put(this.baseApiUrl + '/admin/experts/areas', request).toPromise();
  }

  public setExpertAvailabilityAsync(request: AdminSetAvailabilityRequest) {
    return this.http.put(this.baseApiUrl + '/admin/experts/availability', request).toPromise();
  }

  public updateUserAsync(request: AdminUserUpdateRequest) {
    return this.http.put(this.baseApiUrl + '/admin/users', request).toPromise();
  }

  public async deleteAsync(address: string, transactionHash: string): Promise<void> {
    const queryString = '?address=' + address + '&transactionHash=' + transactionHash;
    await this.http.delete(this.baseApiUrl + '/admin' + queryString).toPromise();
  }

  public getAllAsync(): Promise<CollectionResponse<AdminResponse>> {
    return this.http.get<CollectionResponse<AdminResponse>>(this.baseApiUrl + '/admin').toPromise();
  }

  public getAllUsersAsync(offset: number, count: number): Promise<CollectionResponse<UserResponse>> {
    const parameters = new HttpParams()
      .append('offset', offset.toString())
      .append('count', count.toString());
    return this.http.get<CollectionResponse<UserResponse>>(`${this.baseApiUrl}/admin/users`, {
      params: parameters
    }).toPromise();
  }
}
