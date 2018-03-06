import { Component, OnInit } from '@angular/core';
import {LazyLoadEvent} from 'primeng/api';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertHistoryOffer} from '../../../api/expert/expert-history-offer';
import {ExpertOfferStatus} from '../../../services/expert/expert-offer-status.enum';

@Component({
  selector: 'app-expert-offers-history',
  templateUrl: './expert-offers-history.component.html',
  styleUrls: ['./expert-offers-history.component.css']
})
export class ExpertOffersHistoryComponent implements OnInit {

    public offers: ExpertHistoryOffer[];

    public renderTableRows(historyOfferItems: ExpertHistoryOffer[]) {
        this.offers = [];
        for (const offer of historyOfferItems) {
            const historyOffer = <ExpertHistoryOffer>{
                id: offer.id,
                name: offer.name,
                date: offer.date,
                status: offer.status,
                earned: offer.earned
            };
            this.offers.push(historyOffer);
        }
    }

    constructor(private expertApiClient: ExpertApiClient) { }

    public getExpertOfferStatusById(id) {
      return ExpertOfferStatus[id];
    }

    ngOnInit() {
        this.expertApiClient.getMockHestoryOffersListAsync().subscribe(offers => this.offers = offers);
        this.renderTableRows(this.offers);
    }

}
