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
import {Area} from './expert/area';
import {FreeScoringConfirmationModalComponent} from '../components/common/free-scoring-confirmation-modal/free-scoring-confirmation-modal.component';
import {VoteModalData} from '../components/common/vote-modal/vote-modal-data';
import {VoteModalComponent} from '../components/common/vote-modal/vote-modal.component';
import {RegisterModalData} from '../components/common/register-modal/register-modal-data';
import {RegisterModalComponent} from '../components/common/register-modal/register-modal.component';
import {AddAdminModalComponent} from '../components/common/add-admin-modal/add-admin-modal.component';
import {ConfirmEmailModalComponent} from '../components/common/confirm-email/confirm-email-modal.component';
import {CreateNewExpertModalComponent} from '../components/common/create-new-expert-modal/create-new-expert-modal.component';
import {EditExpertModalComponent} from '../components/common/edit-expert-modal/edit-expert-modal.component';
import {ExpertsCountSelectionModalComponent} from '../components/common/experts-count-selection-modal/experts-count-selection-modal.component';
import {ExpertsCountSelectionModalData} from '../components/common/experts-count-selection-modal/experts-count-selection-modal-data';
import {AreaType} from '../api/scoring/area-type.enum';
import {AreaExpertsSettings} from '../components/common/experts-count-selection-modal/area-experts-settings';
import {SetExpertsModalComponent} from '../components/common/set-experts-modal/set-experts-modal.component';
import {ConfirmEmailModalData} from '../components/common/confirm-email/confirm-email-modal-data';
import {ChangeEmailModalComponent} from '../components/common/change-email-modal/change-email-modal.component';

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

  public showSetExpertsDialogAsync(areas: Array<Area>): Promise<any> {
    return this.openModal(SetExpertsModalComponent, {areas: areas})
      .afterClosed()
      .toPromise<any>();
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

  public showCreateAdminDialogAsync(): Promise<string> {
    return this.openModal(AddAdminModalComponent, {})
      .afterClosed()
      .toPromise<string>();
  }

  public showExpertsCountSelectionDialogAsync(areas: Array<AreaType>): Promise<Array<number>> {
    const data = <ExpertsCountSelectionModalData>{
      settings: areas.map(a => <AreaExpertsSettings>{area: a, expertsCount: 3})
    };
    return this.openModal(ExpertsCountSelectionModalComponent, data, true)
      .afterClosed()
      .toPromise<Array<number>>();
  }

  public showVoteDialogAsync(projectName: string,
                             currentBalance: number,
                             currentVoteBalance: number,
                             currentSprintEndDate: Date): Promise<number> {
    const data = <VoteModalData>{
      projectName: projectName,
      currentBalance: currentBalance,
      currentVoteBalance: currentVoteBalance,
      currentSprintEndDate: currentSprintEndDate
    };

    return this.openModal(VoteModalComponent, data)
      .afterClosed()
      .toPromise<number>();
  }

  public async showRegisterDialogAsync() {
    const data = <RegisterModalData>{
      email: ''
    };
    return this.openModal(RegisterModalComponent, data)
      .afterClosed()
      .toPromise<string>();
  }

  public async showChangeEmailDialogAsync(): Promise<string> {
    return this.openModal(ChangeEmailModalComponent, {})
      .afterClosed()
      .toPromise<string>();
  }

  public showConfirmEmailDialogAsync(email: string): Promise<boolean> {
    return this.dialog.open(ConfirmEmailModalComponent, {
      data: <ConfirmEmailModalData>{
        email: email
      }, width: '30em'
    })
      .afterClosed()
      .toPromise<boolean>();
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

  public showCreateNewExpertModal(): MatDialogRef<CreateNewExpertModalComponent> {
    return this.openModal(CreateNewExpertModalComponent, {});
  }

  public showEditExpertModal(expertData: any): MatDialogRef<EditExpertModalComponent> {
    return this.openModal(EditExpertModalComponent, expertData);
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
