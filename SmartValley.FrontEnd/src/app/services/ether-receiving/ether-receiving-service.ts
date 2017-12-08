import {Injectable} from '@angular/core';
import {DialogService} from '../dialog-service';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {NotificationsService} from 'angular2-notifications';
import {Web3Service} from '../web3-service';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class EtherReceivingService {
  constructor(private dialogService: DialogService,
              private balanceApiClient: BalanceApiClient,
              private notificationsService: NotificationsService,
              private web3Service: Web3Service,
              private translateService: TranslateService) {
  }

  public async receiveAsync(): Promise<void> {
    const transactionHash = (await this.balanceApiClient.receiveEtherAsync()).transactionHash;
    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('EtherReceiving.Dialog'),
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);
      this.notificationsService.success(this.translateService.instant('EtherReceiving.Success'));
    } catch (e) {
      this.notificationsService.error(this.translateService.instant('EtherReceiving.Error'));
    }

    transactionDialog.close();
  }
}
