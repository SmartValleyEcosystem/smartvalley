import {Component, OnInit} from '@angular/core';
import {AdminApiClient} from '../../../api/admin/admin-api-client';
import {AdminContractClient} from '../../../services/contract-clients/admin-contract-client';
import {NotificationsService} from 'angular2-notifications';
import {UserApiClient} from '../../../api/user/user-api-client';
import {DialogService} from '../../../services/dialog-service';
import {UserContext} from '../../../services/authentication/user-context';
import {TranslateService} from '@ngx-translate/core';
import {AdminResponse} from '../../../api/admin/admin-response';
import {isNullOrUndefined} from 'util';
import {LazyLoadEvent} from 'primeng/api';

@Component({
  selector: 'app-admin-admins-list',
  templateUrl: './admin-admins-list.component.html',
  styleUrls: ['./admin-admins-list.component.scss']
})
export class AdminAdminsListComponent implements OnInit {

  public admins: Array<AdminResponse> = [];

  constructor(private adminApiClient: AdminApiClient,
              private adminContractClient: AdminContractClient,
              private notificationsService: NotificationsService,
              private userApiClient: UserApiClient,
              private dialogService: DialogService,
              private userContext: UserContext,
              private translateService: TranslateService) {
  }

  private async updateAdminsAsync() {
    const response = await this.adminApiClient.getAllAsync();
    this.admins = response.items;
  }

  async createAsync() {
    const address = await this.dialogService.showCreateAdminDialogAsync();
    if (isNullOrUndefined(address)) {
      return;
    }

    const isAdmin = await this.adminContractClient.isAdminAsync(address);
    if (isAdmin) {
      this.notificationsService.warn(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('AdminPanel.AlreadyAdmin'));
      return;
    }

    const user = await this.userApiClient.getByAddressAsync(address);
    if (user.address == null) {
      this.notificationsService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Common.UserNotFound'));
      return;
    }
    const transactionHash = await this.adminContractClient.addAsync(address);
    await this.adminApiClient.addAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  async deleteAsync(address: string) {
    const fromAddress = await this.userContext.getCurrentUser().account;
    const transactionHash = await this.adminContractClient.deleteAsync(address, fromAddress);
    await this.adminApiClient.deleteAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  async ngOnInit(): Promise<void> {
    await this.updateAdminsAsync();
  }

  public async getAdminList(event: LazyLoadEvent) {
    await this.updateAdminsAsync();
  }
}
