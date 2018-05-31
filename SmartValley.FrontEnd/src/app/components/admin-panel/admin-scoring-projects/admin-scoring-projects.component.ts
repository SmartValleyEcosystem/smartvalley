import {Component, OnInit} from '@angular/core';
import {ScoringProjectStatus} from '../../../services/scoring-project-status.enum';
import {AdminScoringProjectItem} from './admin-scoring-project-item';
import {AdminScoringProjectAreaExpertItem} from './admin-scoring-project-area-expert-item';
import {Area} from '../../../services/expert/area';
import {SelectItem} from 'primeng/api';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {BlockiesService} from '../../../services/blockies-service';
import * as moment from 'moment';
import {ScoringOffersManagerContractClient} from '../../../services/contract-clients/scoring-offers-manager-contract-client.service';
import {ScoreColorsService} from '../../../services/project/score-colors.service';
import {GetScoringProjectsRequest} from '../../../api/project/get-scoring-projects-request';
import {StatusRequest} from '../../../api/project/status-request';
import {DialogService} from '../../../services/dialog-service';
import {isNullOrUndefined} from 'util';
import {TranslateService} from '@ngx-translate/core';
import {AreaService} from '../../../services/expert/area.service';
import {OffersApiClient} from '../../../api/scoring-offer/offers-api-client';

@Component({
  selector: 'app-admin-scoring-projects',
  templateUrl: './admin-scoring-projects.component.html',
  styleUrls: ['./admin-scoring-projects.component.scss']
})
export class AdminScoringProjectsComponent implements OnInit {

  ScoringProjectStatus = ScoringProjectStatus;
  categories: SelectItem[] = [];
  selectedCategories: any[] = [];
  projects: AdminScoringProjectItem[] = [];

  constructor(private projectClient: ProjectApiClient,
              public scoreColorsService: ScoreColorsService,
              private blockiesService: BlockiesService,
              private dialogService: DialogService,
              private scoringExpertsManagerContractClient: ScoringOffersManagerContractClient,
              private translateService: TranslateService,
              private offersApiClient: OffersApiClient,
              private areaService: AreaService) {

    this.categories = [
      {label: this.translateService.instant('AdminScoringProject.All'), value: ScoringProjectStatus.All},
      {label: this.translateService.instant('AdminScoringProject.InProgress'), value: ScoringProjectStatus.InProgress},
      {label: this.translateService.instant('AdminScoringProject.Rejected'), value: ScoringProjectStatus.Rejected},
      {
        label: this.translateService.instant('AdminScoringProject.AcceptedAndDoNotEstimate'),
        value: ScoringProjectStatus.AcceptedAndDoNotEstimate
      }
    ];
  }

  async updateProjectsAsync() {
    const categories = this.selectedCategories.map(i => <StatusRequest>{StatusId: i});
    const getScoringProjectsRequest = <GetScoringProjectsRequest>{statuses: categories};
    const projectsResponse = await this.projectClient.getScoringProjectsByCategoriesAsync(getScoringProjectsRequest);
    this.projects = projectsResponse.items.map(i => <AdminScoringProjectItem>{
      projectExternalId: i.projectExternalId,
      imageUrl: this.blockiesService.getImageForAddress(i.address),
      startDate: isNullOrUndefined(i.startDate) ? '' : moment(i.startDate).format('MMMM D, Y'),
      endDate: isNullOrUndefined(i.endDate) ? '' : moment(i.endDate).format('MMMM D, Y'),
      status: this.setStatusText(i.status),
      statusCode: i.status,
      title: i.name,
      areasExperts: i.areasExperts.map(j => <AdminScoringProjectAreaExpertItem>{
        area: <Area>{
          areaType: j.areaType,
          name: this.areaService.areas[j.areaType - 1].name
        },
        acceptedCount: j.acceptedCount,
        requiredCount: j.requiredCount
      })
    });
  }

  async relaunchAsync(projectExternalId: string) {
    const transactionHash = await this.scoringExpertsManagerContractClient.regenerateOffersAsync(projectExternalId);
    if (transactionHash == null) {
      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('AdminScoringProject.Dialog'),
      transactionHash
    );

    await this.offersApiClient.updateOffersAsync(projectExternalId, transactionHash);

    transactionDialog.close();
  }

  setStatusText(status: ScoringProjectStatus): string {
    switch (status) {
      case ScoringProjectStatus.AcceptedAndDoNotEstimate :
        return this.translateService.instant('AdminScoringProject.DontEstimateStatus');
      case ScoringProjectStatus.InProgress :
        return this.translateService.instant('AdminScoringProject.InProgressStatus');
      case ScoringProjectStatus.Rejected :
        return this.translateService.instant('AdminScoringProject.RejectedStatus');
    }
  }

  async onCheckAsync() {
    await this.updateProjectsAsync();
  }

  async ngOnInit() {
    await this.updateProjectsAsync();
  }
}
