import {Component, OnInit} from '@angular/core';
import {AreaService} from '../../../services/expert/area.service';
import {Area} from '../../../services/expert/area';
import {ScoringService} from '../../../services/scoring/scoring.service';
import {Balance} from '../../../services/balance/balance';
import {BalanceService} from '../../../services/balance/balance.service';
import {ActivatedRoute, Router} from '@angular/router';
import {ScoringApiClient} from '../../../api/scoring/scoring-api-client';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {Paths} from '../../../paths';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {TranslateService} from '@ngx-translate/core';
import {NotificationsService} from 'angular2-notifications';

@Component({
  selector: 'app-scoring-payment',
  templateUrl: './scoring-payment.component.html',
  styleUrls: ['./scoring-payment.component.scss']
})
export class ScoringPaymentComponent implements OnInit {

  public areas: Area[];
  public totalExperts = 0;
  public expertsInArea: { [id: number]: number } = {};
  public currentBalance: number;
  public areaCosts: { [id: number]: number } = {};
  public externalId: string;
  public totalSum = 0;
  public projectId: number;

  public minimumExperts = 3;
  public maximumExperts = 6;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private areaService: AreaService,
              private projectApiClient: ProjectApiClient,
              private balanceService: BalanceService,
              private scoringApiClient: ScoringApiClient,
              private scoringService: ScoringService,
              private notificationsService: NotificationsService,
              private translateService: TranslateService) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
  }

  public async ngOnInit() {

    // MVP
    this.minimumExperts = 2;

    this.areas = this.areaService.areas;
    for (let i = 0; this.areas.length > i; i++) {
      this.expertsInArea[this.areas[i].areaType] = this.minimumExperts;
      this.areaCosts[this.areas[i].areaType] = await this.scoringService.getScoringCostInAreaAsync(this.areas[i].areaType);
    }
    this.projectId = +this.route.snapshot.paramMap.get('id');
    const projectSummary: ProjectSummaryResponse = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    this.externalId = projectSummary.externalId;
    this.updateBalance(this.balanceService.balance);
    this.calculateTotalExperts();
  }

  public getExperts(expert: { [id: number]: number }) {
    for (const expertId in this.expertsInArea) {
      if (expert[expertId]) {
        this.expertsInArea[expertId] = expert[expertId];
      }
    }
    this.calculateTotalExperts();
  }

  public calculateTotalExperts() {
    this.totalExperts = 0;
    this.totalSum = 0;
    for (const area of Object.keys(this.expertsInArea)) {
      this.totalSum += (+this.areaCosts[area] * this.expertsInArea[area]);
      this.totalExperts += this.expertsInArea[area];
    }
  }

  private updateBalance(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
    }
  }

  public async payAsync() {
    const areas: number[] = [];
    const areaExpertCounts: number[] = [];
    for (const area of Object.keys(this.expertsInArea)) {
      areas.push(+area);
      areaExpertCounts.push(this.expertsInArea[area]);
    }

    if (areaExpertCounts.some(i => i < this.minimumExperts)) {
      this.notificationsService.error(
        this.translateService.instant('Common.ToFewExpertsErroTitle'),
        this.translateService.instant('Common.ToFewExpertsErrorMessage')
      );
      return;
    }

    const transactionHash = await this.scoringService.startAsync(this.externalId, areas, areaExpertCounts);
    await this.scoringApiClient.startAsync(this.projectId, areas, areaExpertCounts, transactionHash);

    await this.router.navigate([Paths.Project + '/' + this.projectId]);
  }
}
