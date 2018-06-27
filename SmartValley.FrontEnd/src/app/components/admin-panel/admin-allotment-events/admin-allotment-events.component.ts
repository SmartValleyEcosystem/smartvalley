import {Component} from '@angular/core';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';
import {AllotmentEventStatus} from '../../../api/allotment-events/allotment-event-status';
import {LazyLoadEvent} from 'primeng/api';
import {DialogService} from '../../../services/dialog-service';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {isNullOrUndefined} from 'util';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';
import {TransactionApiClient} from '../../../api/transaction/transaction-api-client';
import {TransactionRequest} from '../../../api/transaction/requests/transaction-request';
import {EthereumTransactionTypeEnum} from '../../../api/transaction/ethereum-transaction-type.enum';
import {EthereumTransactionEntityTypeEnum} from '../../../api/transaction/ethereum-transaction-entity-type.enum';
import {EthereumTransactionStatusEnum} from '../../../api/transaction/ethereum-transaction-status.enum';
import {AllotmentEvent} from '../../../services/allotment-event/allotment-event';
import {LinkHelper} from '../../../utils/link-helper';
import {TransactionInfo} from './transaction-info';

@Component({
  selector: 'app-admin-allotment-events',
  templateUrl: './admin-allotment-events.component.html',
  styleUrls: ['./admin-allotment-events.component.scss']
})
export class AdminAllotmentEventsComponent {

  public allotmentEvents: AllotmentEvent[];
  public selectedStatuses: AllotmentEventStatus[] = [];
  public AllotmentEventStatus = AllotmentEventStatus;
  public loading = true;
  public offset = 0;
  public pageSize = 10;
  public transactionsInfo: TransactionInfo[] = [];

  constructor(private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private transactionApiClient: TransactionApiClient,
              private router: Router,
              private linkHelper: LinkHelper,
              private dialogService: DialogService,
              private allotmentEventService: AllotmentEventService) {
  }

  public async sortByStatusAsync(checked: boolean, status: AllotmentEventStatus): Promise<void> {
    if (checked) {
      this.selectedStatuses.push(status);
    } else {
      this.selectedStatuses = this.selectedStatuses.filter(s => s !== status);
    }
    this.offset = 0;
    await this.loadAllotmentEventsAsync();
  }

  private async loadAllotmentEventsAsync(): Promise<void> {
    this.loading = true;
    this.allotmentEvents = await this.allotmentEventService.getAllotmentEventsAsync(this.offset, this.pageSize, this.selectedStatuses);
    for (let i = 0; i < this.allotmentEvents.length; i++) {
      if (this.allotmentEvents[i].eventContractAddress === null) {
        continue;
      }
      const transactionInfo = await this.transactionApiClient.getEthereumTransactionAsync(<TransactionRequest>{
        count: 1,
        entityIds: [this.allotmentEvents[i].id],
        entityTypes: [EthereumTransactionEntityTypeEnum.AllotmentEvent],
        transactionTypes: [EthereumTransactionTypeEnum.DeleteAllotmentEvent,
          EthereumTransactionTypeEnum.EditAllotmentEvent,
          EthereumTransactionTypeEnum.PublishAllotmentEvent,
          EthereumTransactionTypeEnum.StartAllotmentEvent],
        statuses: [EthereumTransactionStatusEnum.InProgress]
      });
      this.allotmentEvents[i].isUpdating = !!transactionInfo.items.length;
      if (transactionInfo.items[transactionInfo.items.length]) {
        this.transactionsInfo.push({
          eventid: this.allotmentEvents[i].id,
          transaction: transactionInfo.items[transactionInfo.items.length].hash
        });
      }
    }
    this.allotmentEvents = await this.allotmentEventService.getAllotmentEventsAsync(this.offset, this.pageSize, this.selectedStatuses);
    this.loading = false;
  }

  public getTransaction(eventId: number): string {
    const transactionInfo = this.transactionsInfo.find(t => t.eventid === eventId);
    if (transactionInfo) {
      return transactionInfo.transaction;
    }
    return '';
  }

  public getEtherscanLink(eventId: number): string {
    const transaction = this.getTransaction(eventId);
    if (transaction) {
      return this.linkHelper.getEtherscanLink(transaction);
    }
    return '';
  }

  public async getAllotmentEventsAsync(event: LazyLoadEvent): Promise<void> {
    this.offset = event.first;
    await this.loadAllotmentEventsAsync();
  }
  public async showStartAllotmentEventModal(allotmentEventData: AllotmentEventResponse) {
    const start = await this.dialogService.showStartAllotmentEventDialogAsync(allotmentEventData);
    if (start) {
      await this.allotmentEventService.startAsync(allotmentEventData.id);
    }
  }

  public async showNewAllotmentEventModalAsync(): Promise<void> {
    const allotmentEventCreated = await this.dialogService.showNewAllotmentEventDialogAsync();
    if (allotmentEventCreated) {
      await this.loadAllotmentEventsAsync();
    }
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async publishAsync(event: AllotmentEventResponse) {
    await this.allotmentEventService.publishAsync(event.id,
      event.name,
      event.tokenContractAddress,
      event.tokenDecimals,
      event.tokenTicker,
      event.finishDate);
  }

  public async showEditAllotmentModal(allotmentEvent: AllotmentEventResponse) {
    const editModal = await this.dialogService.showEditAllotmentEventDialogAsync(allotmentEvent);
    if (editModal) {
      await this.allotmentEventService.editAsync(
        allotmentEvent.id,
        editModal.eventName,
        editModal.tokenAddress,
        editModal.tokenDecimals,
        editModal.ticker,
        editModal.finishDate);
    }
  }

  public async showDeleteAllotmentModalAsync(allotmentEvent: AllotmentEventResponse) {
    const modal = await this.dialogService.showDeleteAllotmentEventModalAsync();
    if (modal) {
      await this.allotmentEventService.removeAsync(allotmentEvent.id);
    }
  }

  public async showReturnAddressModalAsync() {
    const address = await this.allotmentEventsManagerContractClient.getReturnAddressAsync();
    const newAddress = await this.dialogService.showReturnAddressDialogAsync(address);
    if (isNullOrUndefined(newAddress) || newAddress === '' || address === newAddress) {
      return;
    }
    await this.allotmentEventsManagerContractClient.setReturnAddressAsync(newAddress);
  }

  public async showFreezeTimeModalAsync() {
    const freezeTime = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
    const newFreezeTime = await this.dialogService.showSetFreezeTimeDialogAsync(freezeTime);
    if (isNullOrUndefined(newFreezeTime) || freezeTime === newFreezeTime) {
      return;
    }
    await this.allotmentEventsManagerContractClient.setFreezingDurationAsync(newFreezeTime);
  }
}
