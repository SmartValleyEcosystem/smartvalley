import {Component, OnInit} from '@angular/core';
import {ExpertScoringOffer} from '../../../api/expert/expert-scoring-offer';
import {AreaService} from '../../../services/expert/area.service';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {OffersApiClient} from '../../../api/expert/offers-api-client';
import {AreaType} from '../../../api/scoring/area-type.enum';

@Component({
  selector: 'app-expert-work-place',
  templateUrl: './expert-work-place.component.html',
  styleUrls: ['./expert-work-place.component.css']
})
export class ExpertWorkPlaceComponent implements OnInit {

  public expertScoring: ExpertScoringOffer[] = [];

  constructor(private offersApiClient: OffersApiClient,
              private areaService: AreaService,
              private router: Router) {
  }

  async ngOnInit() {
    const offersResponse = await this.offersApiClient.getExpertOffersAsync();
    this.expertScoring = offersResponse.items;
  }

  public navigateToEstimate(id: number, areaType: AreaType) {
    this.router.navigate([Paths.Scoring + '/' + id], {queryParams: {areaType: areaType}});
  }
}
