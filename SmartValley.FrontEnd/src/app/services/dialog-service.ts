import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog, MatDialogRef} from '@angular/material';
import {ReceiveEtherModalComponent} from '../components/common/receive-ether-modal/receive-ether-modal.component';
import {ReceiveSvtModalComponent} from '../components/common/receive-svt-modal/receive-svt-modal.component';
import {SvtWithdrawalConfirmationModalComponent} from '../components/common/svt-withdrawal-confirmation-modal/svt-withdrawal-confirmation-modal.component';
import {SvtWithdrawalConfirmationModalData} from '../components/common/svt-withdrawal-confirmation-modal/svt-withdrawal-confirmation-modal-data';
import {ComponentType} from '@angular/cdk/portal';
import {MetamaskManualModalData} from '../components/common/metamask-manual-modal/metamask-manual-modal-data';
import {MetamaskManualModalComponent} from '../components/common/metamask-manual-modal/metamask-manual-modal.component';
import {TranslateService} from '@ngx-translate/core';
import {AlertModalComponent} from '../components/common/alert-modal/alert-modal.component';
import {AlertModalData} from '../components/common/alert-modal/alert-modal-data';
import {FreeScoringConfirmationModalComponent} from '../components/common/free-scoring-confirmation-modal/free-scoring-confirmation-modal.component';

@Injectable()
export class DialogService {
  constructor(private dialog: MatDialog, private translateService: TranslateService) {
  }

  public showGetEtherDialogAsync(): Promise<boolean> {
    return this.openModalAsync(ReceiveEtherModalComponent, {canReceive: true});
  }

  public async showRinkeByDialogAsync(): Promise<boolean> {
    return this.openModalAsync(ReceiveEtherModalComponent, {canReceive: false});
  }

  public async showSvtDialogAsync(date: string): Promise<boolean> {
    return this.openModalAsync(ReceiveSvtModalComponent, {canReceive: false, date: date});
  }

  public async showGetTokenDialogAsync(): Promise<boolean> {
    return this.openModalAsync(ReceiveSvtModalComponent, {canReceive: true});
  }

  public showTransactionDialog(message: string, transactionHash: string): MatDialogRef<TransactionAwaitingModalComponent> {
    const data = <TransactionAwaitingModalData>{message: message, transactionHash: transactionHash};
    return this.openModal(TransactionAwaitingModalComponent, data, true);
  }

  public showSvtWithdrawalConfirmationDialogAsync(amountToWithdraw: number): Promise<boolean> {
    const data = <SvtWithdrawalConfirmationModalData>{amount: amountToWithdraw};
    return this.openModalAsync(SvtWithdrawalConfirmationModalComponent, data);
  }

  public showFreeScoringConfirmationDialogAsync(): Promise<boolean> {
    return this.openModalAsync(FreeScoringConfirmationModalComponent, {});
  }

  public showRinkebyAlert(): MatDialogRef<MetamaskManualModalComponent> {
    const data = <MetamaskManualModalData>{
      title: this.translateService.instant('Authentication.WrongNetworkTitle'),
      message: this.translateService.instant('Authentication.WrongNetworkMessage'),
      button: this.translateService.instant('Authentication.WrongNetworkButton'),
      imgUrl: '/assets/img/change_network.png'
    };
    return this.openModal(MetamaskManualModalComponent, data);
  }

  public showUnlockAccountAlert(): MatDialogRef<MetamaskManualModalComponent> {
    const data = <MetamaskManualModalData>{
      title: this.translateService.instant('Authentication.UnlockMetamaskTitle'),
      message: this.translateService.instant('Authentication.UnlockMetamaskMessage'),
      button: this.translateService.instant('Authentication.UnlockMetamaskButton'),
      imgUrl: '/assets/img/unlock_metamask.png'
    };
    return this.openModal(MetamaskManualModalComponent, data);
  }

  public showIncompatibleBrowserAlert(): MatDialogRef<AlertModalComponent> {
    const data = <AlertModalData>{
      title: this.translateService.instant('Authentication.IncompatibleBrowserTitle'),
      message: this.translateService.instant('Authentication.IncompatibleBrowserMessage'),
      button: this.translateService.instant('Authentication.IncompatibleBrowserButton')
    };
    return this.openModal(AlertModalComponent, data);
  }

  private openModal<TComponent, TData>(componentType: ComponentType<TComponent>,
                                       data: TData,
                                       disableClose: boolean = false): MatDialogRef<TComponent> {
    return this.dialog.open(componentType, {disableClose: disableClose, data: data});
  }

  private openModalAsync<TComponent, TData>(componentType: ComponentType<TComponent>,
                                            data: TData,
                                            disableClose: boolean = false): Promise<boolean> {
    return this.openModal(componentType, data, disableClose)
      .afterClosed()
      .toPromise<boolean>();
  }
}
