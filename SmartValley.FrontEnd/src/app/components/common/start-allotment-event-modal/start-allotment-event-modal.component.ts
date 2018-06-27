import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {AllotmentEventsManagerContractClient} from '../../../services/contract-clients/allotment-events-manager-contract-client';
import BigNumber from 'bignumber.js';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';

@Component({
  selector: 'app-start-allotment-event-modal',
  templateUrl: './start-allotment-event-modal.component.html',
  styleUrls: ['./start-allotment-event-modal.component.scss']
})
export class StartAllotmentEventModalComponent implements OnInit {

  public tokenBalance: BigNumber;
  public project: ProjectSummaryResponse;
  public freezeTime: number;

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEventResponse,
              private dialogRef: MatDialogRef<AddAdminModalComponent>,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private notificationService: NotificationsService,
              private translateService: TranslateService,
              private projectApiClient: ProjectApiClient,
              private erc223ContractClient: Erc223ContractClient,
              private allotmentEventService: AllotmentEventService) {
  }

  async ngOnInit() {
    this.tokenBalance = await this.erc223ContractClient.getTokenBalanceAsync(this.data.tokenContractAddress, this.data.eventContractAddress);
    this.project = await this.projectApiClient.getProjectSummaryAsync(this.data.projectId);
    this.freezeTime = await this.allotmentEventsManagerContractClient.getFreezingDurationAsync();
  }

  public async submitAsync() {
    const finishDate = new Date(this.data.finishDate);
    const finishDateWithFreezing = new Date(finishDate).getTime() + this.freezeTime * 24 * 3600 * 1000;

    if (!this.tokenBalance[0]) {
        this.notificationService.error(
            this.translateService.instant('StartAllotmentEventModalComponent.EmptyBalance')
        );
      this.dialogRef.close(false);
      return;
    }
    if ( !this.data.finishDate || (finishDateWithFreezing < Date.now()) ) {
        this.notificationService.error(
            this.translateService.instant('StartAllotmentEventModalComponent.BadDate'),
        );
        this.dialogRef.close(false);
        return;
    }
    await this.allotmentEventService.startAsync(this.data.id);
    this.dialogRef.close(true);
  }
}
