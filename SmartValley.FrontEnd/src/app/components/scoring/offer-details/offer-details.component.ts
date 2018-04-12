import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {Area} from '../../../services/expert/area';
import {AreaService} from '../../../services/expert/area.service';
import {ScoringExpertsManagerContractClient} from '../../../services/contract-clients/scoring-experts-manager-contract-client';
import {OffersApiClient} from '../../../api/scoring-offer/offers-api-client';
import {DialogService} from '../../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';
import {Paths} from '../../../paths';
import {ScoringOfferStatusResponse} from '../../../api/scoring-offer/scoring-offer-status-response';
import {OfferStatus} from '../../../api/scoring-offer/offer-status.enum';

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.scss']
})
export class OfferDetailsComponent implements OnInit {

  public isProjectExists = false;
  public area: Area;
  public project: ProjectSummaryResponse;
  public offer: ScoringOfferStatusResponse;
  public OfferStatus = OfferStatus;

  constructor(private router: Router,
              private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute,
              private areaService: AreaService,
              private scoringExpertsManagerContractClient: ScoringExpertsManagerContractClient,
              private offersApiClient: OffersApiClient,
              private dialogService: DialogService,
              private translateService: TranslateService) {
  }

  async ngOnInit() {
    const projectId = +this.route.snapshot.paramMap.get('id');
    const areaType = +this.route.snapshot.paramMap.get('areaType');
    this.area = this.areaService.areas.find(i => i.areaType === areaType);
    this.project = await this.projectApiClient.getProjectSummaryAsync(projectId);
    this.offer = await this.offersApiClient.getOfferStatusAsync(this.project.id, this.area.areaType);

    if (this.project && this.area && this.project.scoring.id) {
      this.isProjectExists = true;
    }
  }

  public async acceptOfferAsync() {
    const transactionHash = await this.scoringExpertsManagerContractClient.acceptOfferAsync(this.project.externalId, this.area.areaType);

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('OfferDetails.Dialog'),
      transactionHash
    );

    await this.offersApiClient.acceptExpertOfferAsync(transactionHash, this.project.scoring.id, this.area.areaType);

    transactionDialog.close();
    this.router.navigate([Paths.Project + '/' + this.project.id + '/scoring']);
  }

  public async declineOfferAsync() {
    const transactionHash = await this.scoringExpertsManagerContractClient.rejectOfferAsync(this.project.externalId, this.area.areaType);

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('OfferDetails.Dialog'),
      transactionHash
    );

    await this.offersApiClient.declineExpertOfferAsync(transactionHash, this.project.scoring.id, this.area.areaType);

    transactionDialog.close();
    await this.router.navigate([Paths.ScoringList]);
  }

  public navigateToEstimateScoring(projectId: number, area: number) {
    this.router.navigate([Paths.Project + '/' + projectId + '/scoring/' + area]);
  }
}
