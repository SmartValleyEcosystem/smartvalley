import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog} from '@angular/material';
import {ReceiveEtherModalComponent} from '../components/common/receive-ether-modal/receive-ether-modal.component';
import {ReceiveSvtModalComponent} from '../components/common/receive-svt-modal/receive-svt-modal.component';

@Injectable()
export class DialogService {
  constructor(private projectModal: MatDialog) {
  }

  public showGetEtherDialogAsync(): Promise<boolean> {
    return this.projectModal.open(ReceiveEtherModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        canReceive: true
      }
    }).afterClosed()
      .toPromise<boolean>();
  }

  public async showRinkeByDialogAsync(): Promise<boolean> {
    return this.projectModal.open(ReceiveEtherModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        canReceive: false
      }
    }).afterClosed()
      .toPromise<boolean>();
  }

  public async showSvtDialogAsync(date: string): Promise<boolean> {
    return this.projectModal.open(ReceiveSvtModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        canReceive: false,
        date: date
      }
    }).afterClosed()
      .toPromise<boolean>();
  }

  public async showGetTokenDialogAsync(): Promise<boolean> {
    return this.projectModal.open(ReceiveSvtModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        canReceive: true
      }
    }).afterClosed()
      .toPromise<boolean>();
  }

  public showTransactionDialog(message: string, transactionHash: string) {
    return this.projectModal.open(TransactionAwaitingModalComponent, {
      width: '600px',
      data: <TransactionAwaitingModalData>{
        message: message,
        transactionHash: transactionHash
      },
      disableClose: true,
    });
  }
}
