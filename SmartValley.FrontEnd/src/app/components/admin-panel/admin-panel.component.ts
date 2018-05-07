import {Component, OnInit} from '@angular/core';
import {AdminApiClient} from '../../api/admin/admin-api-client';
import {DialogService} from '../../services/dialog-service';
import {AdminContractClient} from '../../services/contract-clients/admin-contract-client';
import {ActivatedRoute} from '@angular/router';
import {AdminResponse} from '../../api/admin/admin-response';
import {NotificationsService} from 'angular2-notifications';
import {UserApiClient} from '../../api/user/user-api-client';
import {TranslateService} from '@ngx-translate/core';
import {UserContext} from '../../services/authentication/user-context';
import {isNullOrUndefined} from 'util';
import {NgbTabChangeEvent} from '@ng-bootstrap/ng-bootstrap';
import {Location} from '@angular/common';
import {Paths} from '../../paths';
import {MatTabChangeEvent} from '@angular/material';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  public admins: Array<AdminResponse> = [];

  public mainTabItems: string[] = ['admins', 'experts', 'scoring', 'feedbacks', 'subscribers'];
  public subTabItems: string[] = ['expertList', 'applications'];

  public selectedMainTab = 0;
  public selectedSubTab = this.subTabItems[0];

  constructor(private location: Location,
              private route: ActivatedRoute,
              private adminApiClient: AdminApiClient,
              private adminContractClient: AdminContractClient,
              private notificationsService: NotificationsService,
              private userApiClient: UserApiClient,
              private dialogService: DialogService,
              private userContext: UserContext,
              private translateService: TranslateService) {
  }

  async ngOnInit(): Promise<void> {
    await this.updateAdminsAsync();
  }

  async createAsync() {
    const address = await this.dialogService.showCreateAdminDialogAsync();
    if (isNullOrUndefined(address)) {
      return;
    }

    const isAddressIsAdmin = await this.adminContractClient.isAdminAsync(address);
    if (isAddressIsAdmin) {
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

  public onMainTabChange($event: MatTabChangeEvent) {
    if ($event.index === 1) {
      this.location.replaceState(Paths.Admin + '/' + this.mainTabItems[$event.index] + '/' + this.subTabItems[0]);
    } else {
      this.location.replaceState(Paths.Admin + '/' + this.mainTabItems[$event.index]);
    }
  }

  public onSubTabChange($event: NgbTabChangeEvent) {
    this.location.replaceState(Paths.Admin + '/experts/' + $event.nextId);
  }

  private async updateAdminsAsync() {
    const response = await this.adminApiClient.getAllAsync();
    this.admins = response.items;

    const selectedMainTabName = this.route.snapshot.paramMap.get('mainTab');
    if (isNullOrUndefined(selectedMainTabName)) {
      this.location.replaceState(Paths.Admin + '/' + this.mainTabItems[0]);
    } else {
      if (this.mainTabItems.includes(selectedMainTabName)) {
        this.selectedMainTab = this.mainTabItems.indexOf(selectedMainTabName);
      }
    }

    const selectedSubTabName = this.route.snapshot.paramMap.get('subTab');
    if (!isNullOrUndefined(selectedSubTabName) && this.subTabItems.includes(selectedSubTabName)) {
      this.selectedSubTab = selectedSubTabName;
    }
  }
}
