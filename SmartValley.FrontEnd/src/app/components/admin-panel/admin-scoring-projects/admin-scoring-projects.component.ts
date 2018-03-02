import {Component, OnInit} from '@angular/core';
import {ScoringProjectStatus} from '../../../services/scoring-project-status.enum';
import {AdminScoringProjectItem} from './admin-scoring-project-item';
import {AdminScoringProjectAreaExpertItem} from './admin-scoring-project-area-expert-item';
import {Area} from '../../../services/expert/area';
import {SelectItem} from 'primeng/api';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {BlockiesService} from '../../../services/blockies-service';
import * as moment from 'moment';
import {ScoringExpertsManagerContractClient} from '../../../services/contract-clients/scoring-experts-manager-contract-client';
import {ProjectService} from '../../../services/project-service';
import {GetScoringProjectsRequest} from '../../../api/project/get-scoring-projects-request';
import {StatusRequest} from '../../../api/project/status-request';
import {DialogService} from '../../../services/dialog-service';
import {isNullOrUndefined} from 'util';
import {TranslateService} from '@ngx-translate/core';
import {ScoringApiClient} from '../../../api/scoring/scoring-api-client';
import {AreaService} from '../../../services/expert/area.service';

@Component({
  selector: 'app-admin-scoring-projects',
  templateUrl: './admin-scoring-projects.component.html',
  styleUrls: ['./admin-scoring-projects.component.css']
})
export class AdminScoringProjectsComponent implements OnInit {

  ScoringProjectStatus = ScoringProjectStatus;
  categories: SelectItem[] = [];
  selectedCategories: any[] = [];
  projects: AdminScoringProjectItem[] = [];

  constructor(private projectClient: ProjectApiClient,
              public projectService: ProjectService,
              private blockiesService: BlockiesService,
              private dialogService: DialogService,
              private scorintExpertsManagerContractClient: ScoringExpertsManagerContractClient,
              private translateService: TranslateService,
              private scoringApiClient: ScoringApiClient,
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
    const projectsResponse = await this.projectClient.getScoringProjectsByCategoriesAsync(<GetScoringProjectsRequest>{statuses: categories});
    this.projects = projectsResponse.items.map(i => <AdminScoringProjectItem>{
      projectId: i.projectId,
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

  async relaunchAsync(projectId: string) {
    const transactionHash = await this.scorintExpertsManagerContractClient.selectMissingExpertsAsync(projectId);
    if (transactionHash == null) {
      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('AdminScoringProject.Dialog'),
      transactionHash
    );

    await this.scoringApiClient.updateOffersAsync(projectId, transactionHash);

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

  async setExpertsAsync(projectId: string) {
    const project = this.projects.find(i => i.projectId === projectId);
    const areas = project.areasExperts.filter(i => i.acceptedCount < i.requiredCount).map(i => i.area);
    const areaExperts = await this.dialogService.showSetExpertsDialogAsync(areas);
    if (isNullOrUndefined(areaExperts)) {
      return;
    }
    const areaTypes = areaExperts.map(i => i.areaType);
    const addresses = areaExperts.map(i => i.address);
    const transactionHash = await this.scorintExpertsManagerContractClient.setExpertsAsync(projectId, areaTypes, addresses);
    if (transactionHash == null) {
      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('AdminScoringProject.Dialog'),
      transactionHash
    );

    await this.scoringApiClient.updateOffersAsync(projectId, transactionHash);

    transactionDialog.close();
  }


  async onCheckAsync() {
    await this.updateProjectsAsync();
  }

  async ngOnInit() {
    await this.updateProjectsAsync();
  }

}
