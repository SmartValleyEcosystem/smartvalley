import {Injectable} from '@angular/core';
import {DialogService} from './dialog-service';
import {BalanceApiClient} from '../api/balance/balance-api-client';
import {NotificationsService} from 'angular2-notifications';
import {Web3Service} from './web3-service';

@Injectable()
export class EtherReceivingService {
  constructor(private dialogService: DialogService,
              private balanceApiClient: BalanceApiClient,
              private notificationsService: NotificationsService,
              private web3Service: Web3Service) {
  }

  public async receiveAsync(): Promise<void> {
    const transactionHash = (await this.balanceApiClient.receiveEtherAsync()).transactionHash;

    const transactionDialog = this.dialogService.showTransactionDialog(
      'Ether transfer is in progress. Please wait for completion of transaction.',
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);

      this.notificationsService.success('Ether transfer completed.');
    } catch (e) {
      this.notificationsService.error('Ether transfer failed. Please try again later.');
    }

    transactionDialog.close();
  }
}
