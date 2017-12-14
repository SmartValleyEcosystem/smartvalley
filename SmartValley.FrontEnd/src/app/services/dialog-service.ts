import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog, MatDialogRef} from '@angular/material';
import {GetEtherModalComponent} from '../components/common/get-ether-modal/get-ether-modal.component';
import {GetSVTModalComponent} from '../components/common/get-token-modal/get-token-modal.component';

@Injectable()
export class DialogService {
  constructor(private projectModal: MatDialog) {
  }

  public async showGetEtherDialog(): Promise<boolean> {
    const dialog = this.projectModal.open(GetEtherModalComponent, {
      width: '600px',
      disableClose: false
    });

    const dialogResult = await dialog.afterClosed().toPromise<boolean>();
    if (dialogResult) return true;
    else return false;
  }

  public async showGetTokenDialog(): Promise<boolean> {
    const dialog = this.projectModal.open(GetSVTModalComponent, {
      width: '600px',
      disableClose: false
    });

    const dialogResult = await dialog.afterClosed().toPromise<boolean>();
    if (dialogResult) return true;
    else return false;
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
