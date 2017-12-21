import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog} from '@angular/material';
import {TranslateService} from '@ngx-translate/core';
import {ReceiveEtherModalComponent} from '../components/common/receive-ether-modal/receive-ether-modal.component';
import {ReceiveSvtModalComponent} from '../components/common/receive-svt-modal/receive-svt-modal.component';

@Injectable()
export class DialogService {
  constructor(private projectModal: MatDialog,
              private translateService: TranslateService) {
  }

  public showGetEtherDialog(): Promise<boolean> {
    return this.projectModal.open(ReceiveEtherModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        carReceive: true,
        title: this.translateService.instant('GetEtherModal.Title'),
        content: this.translateService.instant('GetEtherModal.Content'),
        buttonText: this.translateService.instant('GetEtherModal.GetButton')
      }
    }).afterClosed().toPromise<boolean>();
  }

  public async showRinkeByDialog(): Promise<boolean> {
    return this.projectModal.open(ReceiveEtherModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        carReceive: false,
        title: this.translateService.instant('RinkeByModal.Title'),
        content: this.translateService.instant('RinkeByModal.Content'),
        link: this.translateService.instant('RinkeByModal.Link')
      }
    }).afterClosed().toPromise<boolean>();
  }

  public async showSVTDialog(date: string): Promise<boolean> {
    return this.projectModal.open(ReceiveSvtModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        carReceive: false,
        title: this.translateService.instant('CanNotReceiveSVTModal.Title'),
        content: this.translateService.instant('CanNotReceiveSVTModal.Content'),
        date: date
      }
    }).afterClosed().toPromise<boolean>();
  }

  public async showGetTokenDialog(): Promise<boolean> {
    return this.projectModal.open(ReceiveSvtModalComponent, {
      width: '600px',
      disableClose: false,
      data: {
        carReceive: true,
        title: this.translateService.instant('CanReceiveSVTModal.Title'),
        content: this.translateService.instant('CanReceiveSVTModal.Content'),
        buttonText: this.translateService.instant('CanReceiveSVTModal.GetButton')
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
