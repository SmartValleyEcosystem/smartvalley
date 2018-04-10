import {Component, OnInit} from '@angular/core';
import {AreaService} from '../../../services/expert/area.service';
import {Area} from '../../../services/expert/area';
import {ScoringService} from '../../../services/scoring/scoring.service';
import {Balance} from '../../../services/balance/balance';
import {BalanceService} from '../../../services/balance/balance.service';
import {ActivatedRoute, Router} from '@angular/router';
import {ScoringApiClient} from '../../../api/scoring/scoring-api-client';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectDetailsResponse} from '../../../api/project/project-details-response';
import {Paths} from '../../../paths';

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
              private scoringService: ScoringService) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
  }

  public async ngOnInit() {
    this.areas = this.areaService.areas;
    for (let i = 0; this.areas.length > i; i++) {
      this.expertsInArea[this.areas[i].areaType] = this.initialExpertsAmount;
      this.areaCosts[this.areas[i].areaType] = await this.scoringService.getScoringCostInAreaAsync(this.areas[i].areaType);
    }
    this.projectId = +this.route.snapshot.paramMap.get('id');
    const projectDetails: ProjectDetailsResponse = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.externalId = projectDetails.externalId;
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
    await this.scoringApiClient.startAsync(this.externalId, areas, areaExpertCounts, transactionHash);
    this.router.navigate([Paths.MyProject + '/' + this.projectId]);
  }
}
