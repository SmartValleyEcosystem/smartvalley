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
import {AddAdminModalComponent} from '../components/common/add-admin-modal/add-admin-modal.component';
import {CreateNewExpertModalComponent} from '../components/common/create-new-expert-modal/create-new-expert-modal.component';
import {EditExpertModalComponent} from '../components/common/edit-expert-modal/edit-expert-modal.component';
import {SetExpertsModalComponent} from '../components/common/set-experts-modal/set-experts-modal.component';
import {EditExpertModalData} from '../components/common/edit-expert-modal/edit-expert-modal-data';
import {WelcomeModalComponent} from '../components/common/welcome-modal/welcome-modal.component';
import {WelcomeModalData} from '../components/common/welcome-modal/welcome-modal-data';
import {DeleteProjectModalComponent} from '../components/common/delete-project-modal/delete-project-modal.component';
import {WaitingModalComponent} from '../components/common/waiting-modal/waiting-modal.component';
import {ChangeStatusModalComponent} from '../components/common/change-status-modal/change-status-modal.component';
import {SubscribeModalComponent} from '../components/common/subscribe-modal/subscribe-modal.component';
import {SubscribeRequest} from '../components/common/subscribe-modal/subscribe-data';
import {FeedbackModalComponent} from '../components/common/feedback-modal/feedback-modal.component';
import {FeedbackRequest} from '../components/common/feedback-modal/feedback';
import {PrivateScoringModalComponent} from '../components/common/private-scoring-modal/private-scoring-modal.component';
import {EditAllotmentEventModalComponent} from '../components/common/edit-allotment-event-modal/edit-allotment-event-modal.component';
import {StartAllotmentEventModalComponent} from '../components/common/start-allotment-event-modal/start-allotment-event-modal.component';
import {NewAllotmentEventModalComponent} from '../components/common/new-allotment-event-modal/new-allotment-event-modal.component';
import {AllotmentEventResponse} from '../api/allotment-events/responses/allotment-event-response';
import {EditAllotmentRequest} from '../api/allotment-events/request/edit-allotment-request';
import {AllotmentEventParticipateModalComponent} from '../components/common/allotment-event-participate-modal/allotment-event-participate-modal.component';
import {AllotmentEventParticipateDialogData} from '../components/common/allotment-event-participate-modal/allotment-event-participate-dialog-data';
import {SetFreezeTimeModalComponent} from '../components/common/set-freeze-time-modal/set-freeze-time-modal.component';
import {ReturnAddressModalComponent} from '../components/common/return-address-modal/return-address-modal.component';
import {SetFreezeTimeModalData} from '../components/common/set-freeze-time-modal/set-freeze-time-modal-data';
import {ReturnAddressModalData} from '../components/common/return-address-modal/return-address-modal-data';
import {ReceiveTokensModalComponent} from '../components/common/receive-tokens-modal/receive-tokens-modal.component';
import {ReceiveTokensModalData} from '../components/common/receive-tokens-modal/receive-tokens-modal-data';
import {DeleteAllotmentEventModalComponent} from '../components/common/delete-allotment-event-modal/delete-allotment-event-modal.component';
import BigNumber from 'bignumber.js';

@Injectable()
export class DialogService {
  constructor(private dialog: MatDialog, private translateService: TranslateService) {
  }

  public showGetEtherDialogAsync(alreadyReceived: boolean): Promise<boolean> {
    return this.openModalAsync(ReceiveEtherModalComponent, {canReceive: !alreadyReceived});
  }

  public showSetFreezeTimeDialogAsync(freezeTime: number): Promise<number> {
    return this.openModal(SetFreezeTimeModalComponent, <SetFreezeTimeModalData>{freezeTime: freezeTime}, false, '440px')
      .afterClosed()
      .toPromise<number>();
  }

  public showReturnAddressDialogAsync(returnAddress: string): Promise<string> {
    return this.openModal(ReturnAddressModalComponent, <ReturnAddressModalData>{returnAddress: returnAddress}, false, '440px')
      .afterClosed()
      .toPromise<string>();
  }

  public async showReceiveTokensModalAsync(totalTokens: BigNumber, totalBet: BigNumber, userBet: BigNumber, userTokens: number, tokenTicker: string): Promise<boolean> {
    return this.openModal(ReceiveTokensModalComponent, <ReceiveTokensModalData> {
      totalTokens: totalTokens,
      totalBet: totalBet,
      userBet: userBet,
      userTokens: userTokens,
      tokenTicker: tokenTicker
    }, false, '460px')
      .afterClosed()
      .toPromise<boolean>();
  }

  public showTransactionDialog(message: string, transactionHash: string): MatDialogRef<TransactionAwaitingModalComponent> {
    const data = <TransactionAwaitingModalData>{message: message, transactionHash: transactionHash};
    return this.openModal(TransactionAwaitingModalComponent, data, true);
  }

  public showCreateAdminDialogAsync(): Promise<string> {
    return this.openModal(AddAdminModalComponent, {})
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

  public async showDeleteProjectModalAsync(): Promise<boolean> {
    return this.openModalAsync(DeleteProjectModalComponent, {});
  }

  public async showDeleteAllotmentEventModalAsync(): Promise<boolean> {
    return this.openModalAsync(DeleteAllotmentEventModalComponent, {});
  }

  public async changeStatusDialogAsync(activityStatus: boolean, address: string): Promise<boolean> {
    return this.openModalAsync(ChangeStatusModalComponent, {
      title: this.translateService.instant('Header.StatusChanging'),
      message: activityStatus
        ? this.translateService.instant('Header.DialogDescriptionToActive')
        : this.translateService.instant('Header.DialogDescriptionToInactive'),
      button: activityStatus
        ? this.translateService.instant('Header.ChangeToActive')
        : this.translateService.instant('Header.ChangeToInactive'),
      close: this.translateService.instant('Header.NoClose'),
      activity: activityStatus,
      expertAddress: address
    });
  }

  public async showWaitingModal(): Promise<boolean> {
    return this.openModalAsync(WaitingModalComponent, {});
  }

  public showSendReportDialog(): MatDialogRef<AlertModalComponent> {
    const data = <AlertModalData>{
      title: this.translateService.instant('ExpertScoring.ModalTitle'),
      message: this.translateService.instant('ExpertScoring.ModalDescription'),
      button: this.translateService.instant('ExpertScoring.ModalOk')
    };
    return this.openModal(AlertModalComponent, data);
  }

  public async showPrivateScoringApplicationDialog(): Promise<boolean> {
    return this.openModalAsync(PrivateScoringModalComponent, {});
  }

  public async showNewAllotmentEventDialog(): Promise<boolean> {
    return this.openModalAsync(NewAllotmentEventModalComponent, {});
  }

  public async showSubscribeDialog(): Promise<SubscribeRequest> {
    return this.openModalAsync(SubscribeModalComponent, {});
  }

  public async showEditAllotmentEventDialog(editData: AllotmentEventResponse): Promise<EditAllotmentRequest> {
    return this.openModalAsync(EditAllotmentEventModalComponent, editData);
  }

  public async showStartAllotmentEventDialog(allotmenEventData: AllotmentEventResponse): Promise<boolean> {
    return this.openModalAsync(StartAllotmentEventModalComponent, allotmenEventData);
  }

  public async showFeedbackDialog(): Promise<FeedbackRequest> {
    return this.openModalAsync(FeedbackModalComponent, {});
  }

  public async showParticipateDialog(participateDialogData: AllotmentEventParticipateDialogData): Promise<number> {
      return this.openModalAsync(AllotmentEventParticipateModalComponent, participateDialogData);
  }

  private openModal<TComponent, TData>(componentType: ComponentType<TComponent>,
                                       data: TData,
                                       disableClose: boolean = false,
                                       width: string = null): MatDialogRef<TComponent> {
    return this.dialog.open(componentType, {disableClose: disableClose, data: data, width: width == null ? '' : width});
  }

  private openModalAsync<TComponent, TData>(componentType: ComponentType<TComponent>,
                                            data: TData,
                                            disableClose: boolean = false): Promise<any> {
    return this.openModal(componentType, data, disableClose)
      .afterClosed()
      .toPromise<any>();
  }
}
