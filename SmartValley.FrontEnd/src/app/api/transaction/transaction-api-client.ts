import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {TransactionResponse} from './responses/transaction-response';
import {TransactionRequest} from './requests/transaction-request';
import {CollectionResponse} from '../collection-response';

@Injectable()
export class TransactionApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getEthereumTransactionsAsync(options: TransactionRequest):  Promise<CollectionResponse<TransactionResponse>> {
    let parameters = new HttpParams();

    if (options.userIds && options.userIds.length) {
        options.userIds.forEach(userId => parameters = parameters.append('userIds', userId.toString()));
    }
    if (options.entityIds && options.entityIds.length) {
        options.entityIds.forEach(entityId => parameters = parameters.append('entityIds', entityId.toString()));
    }
    if (options.entityTypes && options.entityTypes.length) {
        options.entityTypes.forEach(entityType => parameters = parameters.append('entityTypes', entityType.toString()));
    }
    if (options.transactionTypes && options.transactionTypes.length) {
        options.transactionTypes.forEach(transactionType => parameters = parameters.append('transactionTypes', transactionType.toString()));
    }
    if (options.statuses && options.statuses.length) {
        options.statuses.forEach(status => parameters = parameters.append('statuses', status.toString()));
    }

    return await this.http.get<CollectionResponse<TransactionResponse>>(this.baseApiUrl + '/transactions', {
      params: parameters
    }).toPromise();
  }
}
