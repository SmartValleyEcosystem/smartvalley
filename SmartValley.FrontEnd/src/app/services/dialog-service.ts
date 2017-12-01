import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {BalanceApiClient} from '../api/balance/balance-api-client';
import {Web3Service} from './web3-service';
import {NotificationsService} from 'angular2-notifications';
import {MatDialog, MatDialogRef} from '@angular/material';
import {GetEtherModalComponent} from '../components/common/get-ether-modal/get-ether-modal.component';


@Injectable()
export class DialogService {
  constructor(private balanceApiClient: BalanceApiClient,
              private web3Service: Web3Service,
              private projectModal: MatDialog,
              private notificationsService: NotificationsService) {
  }


  public showGetEtherModal(): MatDialogRef<GetEtherModalComponent> {
    return this.projectModal.open(GetEtherModalComponent, {
      width: '600px',
      disableClose: true,
    });
  }

  public async showReceiveEthDialog() {
    const transactionHash = (await this.balanceApiClient.receiveEtherAsync()).transactionHash;

    this.openProjectModal(
      'Ether transfer is in progress. Please wait for completion of transaction.',
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);

      this.notificationsService.success('Ether transfer completed.');
    } catch (e) {
      this.notificationsService.error('Ether transfer failed. Please try again later.');
    }
    this.projectModal.closeAll();
  }

  private openProjectModal(message: string, transactionHash: string) {
    this.projectModal.open(TransactionAwaitingModalComponent, {
      width: '600px',
      data: <TransactionAwaitingModalData>{
        message: message,
        transactionHash: transactionHash
      },
      disableClose: true,
    });
  }
}
