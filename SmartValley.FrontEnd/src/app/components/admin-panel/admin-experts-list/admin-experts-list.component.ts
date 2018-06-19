import {Component, OnInit} from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {LazyLoadEvent} from 'primeng/api';
import {DialogService} from '../../../services/dialog-service';
import {ExpertDeleteRequest} from '../../../api/expert/expert-delete-request';
import {CollectionResponse} from '../../../api/collection-response';
import {ExpertResponse} from '../../../api/expert/expert-response';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {EditExpertModalData} from '../../common/edit-expert-modal/edit-expert-modal-data';
import {OffersApiClient} from '../../../api/scoring-offer/offers-api-client';
import {OfferStatus} from '../../../api/scoring-offer/offer-status.enum';
import {OffersQuery} from '../../../api/scoring-offer/offers-query';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {ExpertService} from '../../../services/expert/expert.service';

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
  public experts: ExpertResponse[] = [];
  public transactionHash: string;

  constructor(private expertApiClient: ExpertApiClient,
              private notificationsService: NotificationsService,
              private translateService: TranslateService,
              private offersApiClient: OffersApiClient,
              private expertService: ExpertService,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private dialogService: DialogService) {
  }

  public async getExpertList(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadExpertsAsync();
  }

  public async showDialogToCreateNewExpert() {
    await this.dialogService.showCreateNewExpertModal();
  }

  async ngOnInit() {
    await this.loadExpertsAsync();
  }

  private async loadExpertsAsync(): Promise<void> {
    this.loading = true;
    const getExpertsRequest = {
      offset: this.offset,
      count: this.pageSize
    };
    this.expertsResponse = await this.expertApiClient.getExpertsListAsync(getExpertsRequest);
    this.totalRecords = this.expertsResponse.totalCount;
    this.experts = this.expertsResponse.items;
    this.loading = false;
  }

  async checkCanDeleteExpertAsync(address: string): Promise<boolean> {
    const expert = this.experts.firstOrDefault(i => i.address === address);
    if (!expert) {
      return false;
    }
    const offers = await this.offersApiClient.queryAsync(<OffersQuery>{
      offset: 0,
      count: 1,
      expertId: expert.id
    });
    return offers.items.length === 0;
  }

  public async deleteExpertAsync(address) {
    if (await this.checkCanDeleteExpertAsync(address)) {
      await this.expertService.deleteAsync(address);
    } else {
      this.notificationsService.error(
        this.translateService.instant('AdminExpertList.DeleteExpertErrorTitle'),
        this.translateService.instant('AdminExpertList.DeleteExpertErrorMessage')
      );
    }
  }

  public async showExpertEditDialog(rowData: any) {
    await this.dialogService.showEditExpertModal(<EditExpertModalData> {
      address: rowData.address
    });
    await this.loadExpertsAsync();
  }
}
