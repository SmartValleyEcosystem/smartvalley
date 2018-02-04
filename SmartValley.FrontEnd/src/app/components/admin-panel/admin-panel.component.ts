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

  admins: AdminResponse[];

  constructor(private router: Router,
              private adminApiClient: AdminApiClient,
              private adminContractClient: AdminContractClient,
              private authenticationService: AuthenticationService,
              private dialogService: DialogService) {
    this.authenticationService.accountChanged.subscribe(async (user) => await this.checkAdmin(user));
  }

  async ngOnInit(): Promise<void> {
    await this.updateAdminsAsync();
    const currentUser = this.authenticationService.getCurrentUser();
    await this.checkAdmin(currentUser);
  }

  async createAsync() {
    const address = await this.dialogService.showCreateAdminDialogAsync()
    const transactionHash = await this.adminContractClient.addAdminAsync(address);
    await this.adminApiClient.addAdminAsync(address, transactionHash);
    await this.updateAdminsAsync();
  }

  private async checkAdmin(user: User): Promise<void> {
    if (user) {
      const isAdmin = await this.adminContractClient.isAdminAsync(user.account);
      if (!isAdmin) {
        await this.router.navigate([Paths.Root]);
      }
    } else {
      await this.router.navigate([Paths.Root]);
    }
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
