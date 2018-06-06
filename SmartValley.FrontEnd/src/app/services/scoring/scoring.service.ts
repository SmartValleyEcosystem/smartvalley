import {Injectable} from '@angular/core';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {Web3Service} from '../web3-service';
import {AreaType} from '../../api/scoring/area-type.enum';
import {ConverterHelper} from '../converter-helper';
import {NotificationsService} from 'angular2-notifications';
import {ScoringParametersProviderContractClient} from '../contract-clients/scoring-parameters-provider-contract-client';

@Injectable()
export class ScoringService {

  constructor(private scoringManagerContractClient: ScoringManagerContractClient,
              private scoringParametersProviderContractClient: ScoringParametersProviderContractClient,
              private web3Service: Web3Service,
              private notificationService: NotificationsService) {
  }

  public async getTotalScoringCostAsync(areas: Array<number>, areaExpertCounts: Array<number>): Promise<number> {
    let totalCost = 0;
    for (let i = 0; i < areas.length; i++) {
      const estimateRewardInArea = await this.getScoringCostInAreaAsync(areas[i]);
      totalCost += estimateRewardInArea * areaExpertCounts[i];
    }
    return +totalCost.toFixed(5);
  }

  public async getScoringCostInAreaAsync(areaType: AreaType): Promise<number> {
    const cost = await this.scoringParametersProviderContractClient.getAreaRewardAsync(+areaType);
    return this.web3Service.fromWei(ConverterHelper.extractStringValue(cost));
  }

  public async startAsync(projectId: string,
                          areas: Array<number>,
                          areaExpertCounts: Array<number>): Promise<string> {
    const scoringCost = await this.getTotalScoringCostAsync(areas, areaExpertCounts);
    return await this.scoringManagerContractClient.startAsync(projectId, areas, areaExpertCounts, scoringCost);
  }

  public async setScoringCostAsync(areas: Array<number>, costs: Array<number>) {
    const costsWei = costs.map(c => this.web3Service.toWei(c));
    try {
      const transactionHash = await this.scoringParametersProviderContractClient.setAreaRewardsAsync(areas, costsWei);
      await this.web3Service.waitForConfirmationAsync(transactionHash);
      this.notificationService.success('Success', 'Scoring costs updated');
    } catch (e) {
      this.notificationService.error('Error', 'Try again');
    }
  }
}
