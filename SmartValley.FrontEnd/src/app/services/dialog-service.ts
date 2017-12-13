import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog} from '@angular/material';
import {GetEtherModalComponent} from '../components/common/get-ether-modal/get-ether-modal.component';
import {GetSVTModalComponent} from '../components/common/get-token-modal/get-token-modal.component';

@Injectable()
export class DialogService {
  constructor(private projectModal: MatDialog) {
  }

  public showGetEtherDialog(): GetEtherModalComponent {
    return this.projectModal.open(GetEtherModalComponent, {
      width: '600px',
      disableClose: true,
    }).componentInstance;
  }

  public showGetTokenDialog(): GetSVTModalComponent {
    return this.projectModal.open(GetSVTModalComponent, {
      width: '600px',
      disableClose: true,
    }).componentInstance;
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
