import {Injectable} from '@angular/core';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {Web3Service} from '../web3-service';
import {ScoringContractClient} from '../contract-clients/scoring-contract-client';
import {AreaType} from '../../api/scoring/area-type.enum';
import {ConverterHelper} from '../converter-helper';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class ScoringService {

  constructor(private scoringManagerContractClient: ScoringManagerContractClient,
              private scoringContractClient: ScoringContractClient,
              private web3Service: Web3Service,
              private notificationService: NotificationsService) {
  }

  public async getTotalScoringCostAsync(areas: Array<number>, areaExpertCounts: Array<number>): Promise<number> {
    let totalCost = 0;
    for (let i = 0; i < areas.length; i++) {
      const estimateRewardInArea = await this.getScoringCostInAreaAsync(areas[i]);
      const cost = estimateRewardInArea * areaExpertCounts[i];
      totalCost += cost;
    }
    return totalCost;
  }

  public async getScoringCostInAreaAsync(areaType: AreaType): Promise<number> {
    const cost = await this.scoringManagerContractClient.getScoringCostInAreaAsync(+areaType);
    return this.web3Service.fromWei(ConverterHelper.extractStringValue(cost));
  }

  public async startAsync(projectId: string,
                          areas: Array<number>,
                          areaExpertCounts: Array<number>): Promise<string> {
    const scoringCost = await this.getTotalScoringCostAsync(areas, areaExpertCounts);
    return await this.scoringManagerContractClient.startAsync(projectId, areas, areaExpertCounts, scoringCost);
  }

  public startForFreeAsync(projectId: string,
                           sprintAddress: string,
                           areas: Array<number>,
                           areaExpertCounts: Array<number>): Promise<string> {

    return this.scoringManagerContractClient.startForFreeAsync(
      projectId,
      sprintAddress,
      areas,
      areaExpertCounts
    );
  }

  public async setScoringCostAsync(areas: Array<number>, costs: Array<number>) {
    const costsWei = costs.map(c => this.web3Service.toWei(c));
    try {
      const txHash = await this.scoringManagerContractClient.setEstimateRewardsAsync(areas, costsWei);
      await this.web3Service.waitForConfirmationAsync(txHash);
      this.notificationService.success('Success', 'Scoring costs updated');
    } catch (e) {
      this.notificationService.error('Error', 'Try again');
    }
  }

  public async getScoringCostEthByAddressAsync(scoringContractAddress: string): Promise<number> {
    const cost = await this.scoringContractClient.getScoringCostAsync(scoringContractAddress);
    return this.web3Service.fromWei(cost.toString());
  }
}
