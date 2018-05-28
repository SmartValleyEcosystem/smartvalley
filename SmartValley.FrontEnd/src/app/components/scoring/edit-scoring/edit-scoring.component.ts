import {Component, OnInit} from '@angular/core';
import {AreaType} from '../../../api/scoring/area-type.enum';
import {MatCheckboxChange} from '@angular/material';
import {ScoringManagerContractClient} from '../../../services/contract-clients/scoring-manager-contract-client';
import {ScoringApiClient} from '../../../api/scoring/scoring-api-client';
import {Paths} from '../../../paths';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {TranslateService} from '@ngx-translate/core';
import {NotificationsService} from 'angular2-notifications';
import {ScoringExpertsManagerContractClient} from '../../../services/contract-clients/scoring-experts-manager-contract-client';
import {ScoringStatus} from '../../../services/scoring-status.enum';
import {OffersApiClient} from '../../../api/scoring-offer/offers-api-client';
import {CollectionResponse} from '../../../api/collection-response';
import {ExpertResponse} from '../../../api/expert/expert-response';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {GetExpertsRequest} from '../../../api/expert/get-experts-request';
import {LazyLoadEvent} from 'primeng/api';
import {OffersQuery} from '../../../api/scoring-offer/offers-query';
import {ExpertAreaItem} from './expert-area-item';
import {ScoringOfferResponse} from '../../../api/scoring-offer/scoring-offer-response';
import {OfferStatus} from '../../../api/scoring-offer/offer-status.enum';

@Component({
  selector: 'app-edit-scoring',
  templateUrl: './edit-scoring.component.html',
  styleUrls: ['./edit-scoring.component.scss']
})
export class EditScoringComponent implements OnInit {

  public AreaType = AreaType;
  public project: ProjectSummaryResponse;

  public allExpertsResponse: CollectionResponse<ExpertResponse>;
  public experts: ExpertResponse[] = [];
  public offers: ScoringOfferResponse[] = [];
  public expertAreas: ExpertAreaItem[] = [];

  public totalRecords: number;
  public loading: boolean;
  public offset = 0;
  public pageSize = 1000;

  private areas: number[] = [];
  private expertsAddresses: string[] = [];
  private areaExpertCounts: number[] = [];
  private uniqAreas: number[] = [];

  constructor(private expertApiClient: ExpertApiClient,
              private projectApiClient: ProjectApiClient,
              private router: Router,
              private route: ActivatedRoute,
              private scoringApiClient: ScoringApiClient,
              private translateService: TranslateService,
              private offersApiClient: OffersApiClient,
              private notificationsService: NotificationsService,
              private scoringExpertsManagerContractClient: ScoringExpertsManagerContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient) {
  }

  async ngOnInit() {
    const projectId = +this.route.snapshot.paramMap.get('id');
    this.project = await this.projectApiClient.getProjectSummaryAsync(projectId);
  }

  private async loadExpertsAsync(): Promise<void> {
    this.loading = true;

    const getExpertsRequest = <GetExpertsRequest> {
      offset: this.offset,
      count: this.pageSize,
      isInHouse: true
    };
    this.allExpertsResponse = await this.expertApiClient.getExpertsListAsync(getExpertsRequest);
    const offersResponse = await this.offersApiClient.queryAsync(<OffersQuery>{
      offset: this.offset,
      count: this.pageSize,
      scoringId: this.project.scoring.id
    });
    this.expertAreas = offersResponse.items.map(i => <ExpertAreaItem> {expertId: i.expertId, areaId: i.area});
    this.totalRecords = this.allExpertsResponse.totalCount;
    this.experts = this.allExpertsResponse.items;
    this.offers = offersResponse.items;
    this.loading = false;
  }

  public async updateExperts(event: LazyLoadEvent) {
    this.offset = event.first;
    await this.loadExpertsAsync();
  }

  public expertInScoring(expertId: number): boolean {
    return this.expertAreas.some(i => i.expertId === expertId);
  }

  public expertHaveNoArea(expertId: number, areaType: AreaType): boolean {
    const expert = this.experts.firstOrDefault(i => i.id === expertId);
    if (expert) {
      return expert.areas.some(i => <AreaType> i.id === areaType);
    }
    return false;
  }

  public areaInScoring(expertId: number, areaId: AreaType): boolean {
    return this.expertAreas.some(i => i.areaId === areaId && i.expertId === expertId);
  }

  private calculateData() {
    this.uniqAreas = [];
    this.expertsAddresses = [];
    this.areaExpertCounts = [];
    this.areas = [];

    for (const area of this.expertAreas) {
      const privateExpert = this.experts.firstOrDefault(i => i.id === area.expertId);
      if (privateExpert) {
        this.expertsAddresses.push(privateExpert.address);
        this.areas.push(area.areaId);
      }
    }

    for (const area of Object.keys(this.AreaType)) {
      if (+area) {
        this.uniqAreas.push(+area);
        this.areaExpertCounts.push(this.expertCountsByArea(+area));
      }
    }
  }

  public async setExpertsAsync(): Promise<void> {

    this.calculateData();

    if (!this.areaExpertCounts.some(i => i > 0)) {
      this.notificationsService.error(
        this.translateService.instant('EditScoring.ErrorExpertsCountTitle'),
        this.translateService.instant('EditScoring.ErrorExpertsCountMessage')
      );
      return;
    }

    const transactionHash = await this.scoringExpertsManagerContractClient.setExpertsAsync(this.project.externalId, this.areas, this.expertsAddresses);

    await this.offersApiClient.updateOffersAsync(this.project.externalId, transactionHash);

    await this.router.navigate([Paths.Admin + '/scoring/private-scoring']);
  }

  public async startPrivateScoringAsync(): Promise<void> {

    this.calculateData();

    if (!this.areaExpertCounts.some(i => i > 0)) {
      this.notificationsService.error(
        this.translateService.instant('EditScoring.ErrorExpertsCountTitle'),
        this.translateService.instant('EditScoring.ErrorExpertsCountMessage')
      );
      return;
    }

    const transactionHash = await this.scoringManagerContractClient.startPrivateAsync(this.project.externalId, this.areas, this.expertsAddresses);

    await this.scoringApiClient.startAsync(this.project.id, this.uniqAreas, this.areaExpertCounts, transactionHash);

    await this.router.navigate([Paths.Admin + '/scoring/private-scoring']);
  }

  public async finishPrivateScoringAsync(): Promise<void> {

    await this.scoringApiClient.finishAsync(this.project.scoring.id);

    await this.router.navigate([Paths.Admin + '/scoring/private-scoring']);
  }

  public async reopenPrivateScoringAsync(): Promise<void> {

    await this.scoringApiClient.reopenAsync(this.project.scoring.id);

    await this.router.navigate([Paths.Admin + '/scoring/private-scoring']);
  }

  public canStart(): boolean {
    return this.project.scoring && this.project.scoring.scoringStatus === ScoringStatus.FillingApplication;
  }

  public canEdit(): boolean {
    return this.project.scoring.scoringStatus === ScoringStatus.InProgress && this.expertAreas !== this.offers.map(i => <ExpertAreaItem> {
      expertId: i.expertId,
      areaId: i.area
    });
  }

  public canReopen(): boolean {
    return this.project.scoring && this.project.scoring.scoringStatus === ScoringStatus.Finished;
  }

  public canFinished(): boolean {
    this.calculateData();
    return this.project.scoring.scoringStatus === ScoringStatus.InProgress
      && this.offers.every(o => o.offerStatus === OfferStatus.Finished)
      && !this.areaExpertCounts.some(i => i > 0);
  }

  private expertCountsByArea(areaId: AreaType): number {
    return this.expertAreas.filter(i => i.areaId === areaId).length;
  }

  public onExpertChecked(event: MatCheckboxChange, expertId: number) {
    const expert = this.experts.firstOrDefault(i => i.id === expertId);
    if (expert) {
      const areas = this.expertAreas.filter(i => i.expertId === expertId);
      if (!event.checked) {
        this.expertAreas = this.expertAreas.filter(i => i.expertId !== expertId);
        return;
      }
      if (event.checked) {
        const newAreas = expert.areas.map(i => <ExpertAreaItem> {expertId: expert.id, areaId: i.id});
        for (let i = 0; i < newAreas.length; i++) {
          this.expertAreas.push(newAreas[i]);
        }
        return;
      }
    }
  }

  public onAreaChecked(event: MatCheckboxChange, expertId: number, areaType: AreaType) {
    const expert = this.experts.firstOrDefault(i => i.id === expertId);
    if (expert) {
      const area = expert.areas.firstOrDefault(i => <AreaType> i.id === areaType);
      if (area) {
        if (!event.checked) {
          this.expertAreas = this.expertAreas.filter(item => item !== <ExpertAreaItem> {
            areaId: item.areaId,
            expertId: item.expertId
          });
          return;
        }
        if (event.checked) {
          this.expertAreas.push(<ExpertAreaItem> {expertId: expert.id, areaId: areaType});
          return;
        }
      } else {
        if (event.checked) {

        }
      }
    }
  }
}
