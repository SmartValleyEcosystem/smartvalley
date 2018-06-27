import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {TransactionResponse} from './responses/transaction-response';
import {TransactionRequest} from './requests/transaction-request';
import {EthereumTransactionTypeEnum} from './ethereum-transaction-type.enum';
import {EthereumTransactionEntityTypeEnum} from './ethereum-transaction-entity-type.enum';
import {EthereumTransactionStatusEnum} from './ethereum-transaction-status.enum';
import {CollectionResponse} from '../collection-response';
import {UserResponse} from '../user/user-response';

@Injectable()
export class TransactionApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public async getEthereumTransactionAsync(options: TransactionRequest):  Promise<CollectionResponse<TransactionResponse>> {
    let parameters = new HttpParams();

    if (options.userIds && options.userIds.length) {
        parameters = parameters.append('userIds', options.userIds.toString());
    }
    if (options.entityIds && options.entityIds.length) {
        parameters = parameters.append('entityIds', options.entityIds.toString());
    }
    if (options.entityTypes && options.entityTypes.length) {
        parameters = parameters.append('entityTypes', options.entityTypes.toString());
    }
    if (options.transactionTypes && options.transactionTypes.length) {
        parameters = parameters.append('transactionTypes', options.transactionTypes.toString());
    }
    if (options.statuses && options.statuses.length) {
        parameters = parameters.append('statuses', options.statuses.toString());
    }

    return await this.http.get<CollectionResponse<TransactionResponse>>(this.baseApiUrl + '/transactions', {
      params: parameters
    }).toPromise();
  }
}
