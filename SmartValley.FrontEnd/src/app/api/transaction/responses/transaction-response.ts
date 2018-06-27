import {EthereumTransactionEntityTypeEnum} from '../ethereum-transaction-entity-type.enum';
import {EthereumTransactionTypeEnum} from '../ethereum-transaction-type.enum';
import {EthereumTransactionStatusEnum} from '../ethereum-transaction-status.enum';

export interface TransactionResponse {
  id: number;
  userId: number;
  hash: string;
  entityId: number;
  EntityType: EthereumTransactionEntityTypeEnum;
  TransactionType: EthereumTransactionTypeEnum;
  Status: EthereumTransactionStatusEnum;
  Created: Date;
}
