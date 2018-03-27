import {Injectable} from '@angular/core';
import {TransactionAwaitingModalData} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {TransactionAwaitingModalComponent} from '../components/common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog, MatDialogRef} from '@angular/material';
import {ReceiveEtherModalComponent} from '../components/common/receive-ether-modal/receive-ether-modal.component';
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
import {AddAdminModalComponent} from '../components/common/add-admin-modal/add-admin-modal.component';
import {CreateNewExpertModalComponent} from '../components/common/create-new-expert-modal/create-new-expert-modal.component';
import {EditExpertModalComponent} from '../components/common/edit-expert-modal/edit-expert-modal.component';
import {ExpertsCountSelectionModalComponent} from '../components/common/experts-count-selection-modal/experts-count-selection-modal.component';
import {ExpertsCountSelectionModalData} from '../components/common/experts-count-selection-modal/experts-count-selection-modal-data';
import {AreaType} from '../api/scoring/area-type.enum';
import {AreaExpertsSettings} from '../components/common/experts-count-selection-modal/area-experts-settings';
import {SetExpertsModalComponent} from '../components/common/set-experts-modal/set-experts-modal.component';
import {ChangeEmailModalComponent} from '../components/common/change-email-modal/change-email-modal.component';
import {EditExpertModalData} from '../components/common/edit-expert-modal/edit-expert-modal-data';
import {ScoringCostComponent} from '../components/common/scoring-cost-modal/scoring-cost.component';
import {WelcomeModalComponent} from '../components/common/welcome-modal/welcome-modal.component';
import {WelcomeModalData} from '../components/common/welcome-modal/welcome-modal-data';

@Injectable()
export class DialogService {
  constructor(private dialog: MatDialog, private translateService: TranslateService) {
  }

  public showGetEtherDialogAsync(alreadyReceived: boolean): Promise<boolean> {
    return this.openModalAsync(ReceiveEtherModalComponent, {canReceive: !alreadyReceived});
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

  public showFreeScoringConfirmationDialogAsync(): Promise<boolean> {
    return this.openModalAsync(FreeScoringConfirmationModalComponent, {});
  }

  public showCreateAdminDialogAsync(): Promise<string> {
    return this.openModal(AddAdminModalComponent, {})
      .afterClosed()
      .toPromise<string>();
  }

  public showScoringCostDialog(): Promise<void> {
    return this.openModal(ScoringCostComponent, {})
      .afterClosed()
      .toPromise<void>();
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

  public async showChangeEmailDialogAsync(): Promise<string> {
    return this.openModal(ChangeEmailModalComponent, {})
      .afterClosed()
      .toPromise<string>();
  }

  public showMetamaskManualAlert(): MatDialogRef<MetamaskManualModalComponent> {
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

  public showWelcome(): MatDialogRef<WelcomeModalComponent> {
    const data = <WelcomeModalData>{
      title: this.translateService.instant('Authentication.WelcomeTitle'),
      message: this.translateService.instant('Authentication.WelcomeText'),
      button: this.translateService.instant('Authentication.WelcomeButton'),
      imgUrl: '/assets/img/welcome.svg'
    };
    return this.openModal(WelcomeModalComponent, data);
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

  public showEditExpertModal(expertData: EditExpertModalData): Promise<boolean> {
    return this.openModalAsync(EditExpertModalComponent, expertData);
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
