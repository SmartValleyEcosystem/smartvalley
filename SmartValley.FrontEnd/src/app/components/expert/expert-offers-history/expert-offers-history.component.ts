import { Component, OnInit } from '@angular/core';
import {OffersApiClient} from '../../../api/expert/offers-api-client';
import {ExpertHistoryOffer} from '../../../api/expert/expert-history-offer';
import {ExpertOfferStatus} from '../../../services/expert/expert-offer-status.enum';

@Component({
  selector: 'app-expert-offers-history',
  templateUrl: './expert-offers-history.component.html',
  styleUrls: ['./expert-offers-history.component.css']
})
export class ExpertOffersHistoryComponent implements OnInit {

    public offers: ExpertHistoryOffer[] = [];

    constructor(private offersApiClient: OffersApiClient) { }

    public renderTableRows(historyOfferItems: ExpertHistoryOffer[]) {
        this.offers = [];
        for (const offer of historyOfferItems) {
            const historyOffer = <ExpertHistoryOffer>{
                scoringId: offer.scoringId,
                name: offer.name,
                scoringOfferTimestamp: offer.scoringOfferTimestamp,
                offerStatus: offer.offerStatus,
                description: offer.description
            };
            this.offers.push(historyOffer);
        }
    }

    public getExpertOfferStatusById(id) {
      return ExpertOfferStatus[id];
    }

    async ngOnInit() {
        let offersResponse = await this.offersApiClient.getHistoryOffersListAsync();
        this.offers = offersResponse.items;
        this.renderTableRows(this.offers);
    }

}
