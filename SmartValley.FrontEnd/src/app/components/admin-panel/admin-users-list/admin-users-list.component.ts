import {Component, OnInit} from '@angular/core';
import {LazyLoadEvent} from 'primeng/api';
import {AdminApiClient} from '../../../api/admin/admin-api-client';
import {AdminUserUpdateRequest} from '../../../api/admin/admin-user-update-request';
import {UserResponse} from '../../../api/user/user-response';

@Component({
  selector: 'app-admin-users-list',
  templateUrl: './admin-users-list.component.html',
  styleUrls: ['./admin-users-list.component.css']
})
export class AdminUsersListComponent implements OnInit {

  public loading: boolean;
  public offset = 0;
  public pageSize = 10;
  public totalRecords: number;
  public users: UserResponse[] = [];

  constructor(private adminApiClient: AdminApiClient) {
  }


  public async getUserList(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadExpertsAsync();
  }

  public async onChangeUserPrivateAsync(address: string) {
    const canCreatePrivateProjects = this.users.first(i => i.address === address).canCreatePrivateProjects;
    await this.adminApiClient.updateUserAsync(<AdminUserUpdateRequest>{
      address: address,
      canCreatePrivateProjects: canCreatePrivateProjects
    });
  }

  async ngOnInit() {
    await this.loadExpertsAsync();
  }

  private async loadExpertsAsync(): Promise<void> {
    this.loading = true;
    const response = (await this.adminApiClient.getAllUsersAsync(this.offset, this.pageSize));
    this.totalRecords = response.totalCount;
    this.users = response.items;
    this.loading = false;
  }
}
