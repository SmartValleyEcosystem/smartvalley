import {EthereumTransactionStatusEnum} from '../ethereum-transaction-status.enum';
import {EthereumTransactionTypeEnum} from '../ethereum-transaction-type.enum';
import {EthereumTransactionEntityTypeEnum} from '../ethereum-transaction-entity-type.enum';

export interface TransactionRequest {
  userIds?: number[];
  entityIds?: number[];
  entityTypes?: EthereumTransactionEntityTypeEnum[];
  transactionTypes?: EthereumTransactionTypeEnum[];
  statuses?: EthereumTransactionStatusEnum[];
}
