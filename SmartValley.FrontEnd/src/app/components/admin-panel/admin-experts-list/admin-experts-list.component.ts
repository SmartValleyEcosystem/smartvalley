import {Component, OnInit} from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {LazyLoadEvent} from 'primeng/api';
import {AdminExpertItem} from './admin-expert-item';
import {AreaService} from '../../../services/expert/area.service';
import {DialogService} from '../../../services/dialog-service';
import {ExpertDeleteRequest} from '../../../api/expert/expert-delete-request';
import {CollectionResponse} from '../../../api/collection-response';
import {ExpertResponse} from '../../../api/expert/expert-response';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {EditExpertModalData} from "../../common/edit-expert-modal/edit-expert-modal-data";

@Component({
  selector: 'app-admin-experts-list',
  templateUrl: './admin-experts-list.component.html',
  styleUrls: ['./admin-experts-list.component.css']
})
export class AdminExpertsListComponent implements OnInit {
  public totalRecords: number;
  public loading: boolean;
  public offset = 0;
  public pageSize = 10;
  public expertsResponse: CollectionResponse<ExpertResponse>;
  public experts: AdminExpertItem[] = [];
  public transactionHash: string;
  public deleteExpertRequest: ExpertDeleteRequest;

  constructor(private expertApiClient: ExpertApiClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private dialogService: DialogService,
              private areaService: AreaService) {
  }

  public async getExpertList(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadExpertsAsync();
  }

  public renderTableRows(expertResponseItems: ExpertResponse[]) {
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
    await this.loadExpertsAsync();
  }

  private async loadExpertsAsync(): Promise<void> {
    this.loading = true;
    this.expertsResponse = (await this.expertApiClient.getExpertsListAsync(this.offset, this.pageSize));
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
    await this.expertApiClient.deleteAsync(this.deleteExpertRequest);
  }

  public async showExpertEditDialog(rowData: any) {
    await this.dialogService.showEditExpertModal(<EditExpertModalData> {
      address: rowData.address
    });
    await this.loadExpertsAsync();
  }
}
