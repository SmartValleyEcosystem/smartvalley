import {Component, OnInit} from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {LazyLoadEvent} from 'primeng/api';
import {AdminExpertItem} from './admin-expert-item';
import {AreaService} from '../../../services/expert/area.service';
import {DialogService} from '../../../services/dialog-service';
import {DeleteExpertRequest} from '../../../api/expert/delete-expert-request';
import {CollectionResponse} from '../../../api/collection-response';
import {PendingExpertListResponse} from '../../../api/expert/pending-expert-list-response';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';

@Component({
  selector: 'app-admin-experts-list',
  templateUrl: './admin-experts-list.component.html',
  styleUrls: ['./admin-experts-list.component.css']
})
export class AdminExpertsListComponent implements OnInit {
  public totalRecords: number;
  public loading: boolean;
  public currentPage = 0;
  public pageSize = 10;
  public expertsResponse: CollectionResponse<PendingExpertListResponse>;
  public experts: AdminExpertItem[] = [];
  public transactionHash: string;
  public deleteExpertRequest: DeleteExpertRequest;

  constructor(private expertApiClient: ExpertApiClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private dialogService: DialogService,
              private areaService: AreaService) {
  }

  public async getExpertList(event: LazyLoadEvent) {
    this.currentPage = (event.first / event.rows);
    this.loading = true;
    this.expertsResponse = await this.expertApiClient.getExpertsListAsync(this.currentPage, this.pageSize);
    this.renderTableRows(this.expertsResponse.items);
    this.loading = false;

  }

  public renderTableRows(expertResponseItems: PendingExpertListResponse[]) {
    this.experts = [];
    for (const expert of expertResponseItems) {
      const expertItem = <AdminExpertItem>{
        name: expert.name,
        about: expert.about,
        address: expert.address,
        email: expert.email,
        isAvailable: expert.isAvailable,
        areas: this.areaService.getAreasByTypes(expert.areas),
      };
      this.experts.push(expertItem);
    }
  }

  public async showDialogToCreateNewExpert() {
    await this.dialogService.showCreateNewExpertModal();
  }

  async ngOnInit() {
    this.loading = true;
    this.expertsResponse = (await this.expertApiClient.getExpertsListAsync(this.currentPage, this.pageSize));
    this.totalRecords = this.expertsResponse.totalCount;
    this.renderTableRows(this.expertsResponse.items);
    this.loading = false;
  }

  public async deleteExpertAsync(address) {
    this.transactionHash = (await this.expertsRegistryContractClient.removeAsync(address));
    this.deleteExpertRequest = {
      transactionHash: this.transactionHash,
      address: address
    };
    await this.expertApiClient.deleteExpertAsync(this.deleteExpertRequest);
  }

  public async showDialogToEditExpert(rowData: any) {
    await this.dialogService.showEditExpertModal(rowData);
  }
}
