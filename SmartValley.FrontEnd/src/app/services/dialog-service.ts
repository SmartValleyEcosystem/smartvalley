import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog} from '@angular/material';
import {ReceiptModalComponent} from '../components/common/receipt-modal/receipt-modal.component';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class DialogService {
  constructor(private projectModal: MatDialog,
              private translateService: TranslateService) {
  }

  public showGetEtherDialog(): Promise<boolean> {
    return this.projectModal.open(ReceiptModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        title: this.translateService.instant('GetEtherModal.Title'),
        content: this.translateService.instant('GetEtherModal.Content'),
        buttonText: this.translateService.instant('GetEtherModal.GetButton')
      }
    }).afterClosed().toPromise<boolean>();
  }

  public async showGetTokenDialog(): Promise<boolean> {
    return this.projectModal.open(ReceiptModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        title: this.translateService.instant('GetSVTModal.Title'),
        content: this.translateService.instant('GetSVTModal.Content'),
        buttonText: this.translateService.instant('GetSVTModal.GetButton')
      }
    }).afterClosed().toPromise<boolean>();
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
