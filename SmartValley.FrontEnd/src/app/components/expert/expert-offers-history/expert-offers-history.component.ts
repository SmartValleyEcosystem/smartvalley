import {Component, OnInit} from '@angular/core';
import {OffersApiClient} from '../../../api/expert/offers-api-client';
import {ExpertOfferStatus} from '../../../services/expert/expert-offer-status.enum';
import {ScoringOfferResponse} from '../../../api/expert/scoring-offer-response';

@Component({
  selector: 'app-expert-offers-history',
  templateUrl: './expert-offers-history.component.html',
  styleUrls: ['./expert-offers-history.component.css']
})
export class ExpertOffersHistoryComponent implements OnInit {

  public offers: ScoringOfferResponse[] = [];

  constructor(private offersApiClient: OffersApiClient) {
  }

  public getExpertOfferStatusById(id) {
    return ExpertOfferStatus[id];
  }

  async ngOnInit() {
    const offersResponse = await this.offersApiClient.getHistoryOffersListAsync();
    this.offers = offersResponse.items;
  }
}
