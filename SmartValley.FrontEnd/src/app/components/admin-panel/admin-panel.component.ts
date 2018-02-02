import {Component, OnInit} from '@angular/core';
import {AdminApiClient} from '../../api/admin/admin-api-client';
import {DialogService} from '../../services/dialog-service';
import {AdminContractClient} from '../../services/contract-clients/admin-contract-client';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  admins: string[];

  constructor(private adminApiClient: AdminApiClient,
              private adminContractClient: AdminContractClient,
              private dialogService: DialogService) {
  }

  async ngOnInit() {
    await this.updateAdminsAsync();
  }

  async createAsync() {
    const address = await this.dialogService.showCreateAdminDialogAsync()
    const transactionHash = await this.adminContractClient.addAdminAsync(address);
    await this.adminApiClient.addAdminAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  async deleteAsync(address: string) {
    const transactionHash = await this.adminContractClient.deleteAdminAsync(address);
    await this.adminApiClient.deleteAdminAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  private async updateAdminsAsync() {
    const response = await this.adminApiClient.getAllAdminsAsync();
    this.admins = response.items;
  }
}
