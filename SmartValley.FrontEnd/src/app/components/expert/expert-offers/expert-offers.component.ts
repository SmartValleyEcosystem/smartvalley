import {Component, OnInit} from '@angular/core';
import {OffersApiClient} from '../../../api/expert/offers-api-client';
import {ExpertScoringOffer} from '../../../api/expert/expert-scoring-offer';
import {AreaService} from '../../../services/expert/area.service';
import {ScoringExpertsManagerContractClient} from '../../../services/contract-clients/scoring-experts-manager-contract-client';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertAvailabilityStatusResponse} from '../../../api/expert/expert-availability-status-response';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {BlockiesService} from '../../../services/blockies-service';

@Component({
  selector: 'app-expert-offers',
  templateUrl: './expert-offers.component.html',
  styleUrls: ['./expert-offers.component.css']
})
export class ExpertOffersComponent implements OnInit {

  public expertOffers: ExpertScoringOffer[] = [];
  public isAvailable = true;

  constructor(private offersApiClient: OffersApiClient,
              private areaService: AreaService,
              private expertApiClient: ExpertApiClient,
              private blockiesService: BlockiesService,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private scoringExpertsManagerContractClient: ScoringExpertsManagerContractClient) {
  }

  async ngOnInit() {
    const pendingOffers = await this.offersApiClient.getExpertPendingOffersAsync();
    this.expertOffers = pendingOffers.items;
    const availabilityStatusResponse: ExpertAvailabilityStatusResponse = await this.expertApiClient.getAvailabilityStatusAsync();
    this.isAvailable = availabilityStatusResponse.isAvailable;

  }

  public getOfferImage(address) {
    return this.blockiesService.getImageForAddress(address);
  }

  public async acceptOffer(projectId, scoringId, areaId) {
    const transactionHash = (await this.scoringExpertsManagerContractClient.acceptOfferAsync(projectId, areaId));
    await this.offersApiClient.acceptExpertOfferAsync(transactionHash, scoringId, areaId);
  }

  public async declineOffer(projectId, scoringId, areaId) {
    const transactionHash = (await this.scoringExpertsManagerContractClient.rejectOfferAsync(projectId, areaId));
    await this.offersApiClient.declineExpertOfferAsync(transactionHash, scoringId, areaId);
  }

  public async toggleAvailabilityAsync(event) {
    if (event.checked) {
      const transactionHash = await this.expertsRegistryContractClient.expertEnableAsync();
      await this.expertApiClient.switchAvailabilityAsync(transactionHash, true);
    } else {
      const transactionHash = await this.expertsRegistryContractClient.expertDisableAsync();
      await this.expertApiClient.switchAvailabilityAsync(transactionHash, false);
    }
  }
}
