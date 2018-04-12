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
import {DialogService} from '../../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';

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

  public initialExpertsAmount = 3;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private areaService: AreaService,
              private projectApiClient: ProjectApiClient,
              private balanceService: BalanceService,
              private scoringApiClient: ScoringApiClient,
              private scoringService: ScoringService,
              private dialogService: DialogService,
              private translateService: TranslateService) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
  }

  public async ngOnInit() {
    this.areas = this.areaService.areas;
    for (let i = 0; this.areas.length > i; i++) {
      this.expertsInArea[this.areas[i].areaType] = this.initialExpertsAmount;
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
    for (const expert in this.expertsInArea) {
      this.totalSum += (+this.areaCosts[expert] * this.expertsInArea[expert]);
      this.totalExperts += this.expertsInArea[expert];
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
    for (const currentArea in this.expertsInArea) {
      areas.push(+currentArea);
      areaExpertCounts.push(this.expertsInArea[currentArea]);
    }

    const transactionHash = await this.scoringService.startAsync(this.externalId, areas, areaExpertCounts);

    const dialog = this.dialogService.showTransactionDialog(this.translateService.instant('ScoringPayment.Dialog'),
      transactionHash);

    await this.scoringApiClient.startAsync(this.projectId, areas, areaExpertCounts, transactionHash);

    dialog.close();
    this.router.navigate([Paths.Project + '/' + this.projectId]);
  }
}
