import {Component, OnInit} from '@angular/core';
import {AdminApiClient} from '../../api/admin/admin-api-client';
import {DialogService} from '../../services/dialog-service';
import {AdminContractClient} from '../../services/contract-clients/admin-contract-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {AdminResponse} from '../../api/admin/admin-response';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  public admins: Array<AdminResponse> = [];

  constructor(private router: Router,
              private adminApiClient: AdminApiClient,
              private adminContractClient: AdminContractClient,
              private authenticationService: AuthenticationService,
              private dialogService: DialogService) {
  }

  async ngOnInit(): Promise<void> {
    await this.updateAdminsAsync();
  }

  async createAsync() {
    const address = await this.dialogService.showCreateAdminDialogAsync()
    const fromAddress = await this.authenticationService.getCurrentUser().account;
    const transactionHash = await this.adminContractClient.addAsync(address, fromAddress);
    await this.adminApiClient.addAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  async deleteAsync(address: string) {
    const fromAddress = await this.authenticationService.getCurrentUser().account;
    const transactionHash = await this.adminContractClient.deleteAsync(address, fromAddress);
    await this.adminApiClient.deleteAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  private async updateAdminsAsync() {
    const response = await this.adminApiClient.getAllAsync();
    this.admins = response.items;
  }
}
