import { Component, OnInit } from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertScoring} from '../../../api/expert/expert-scoring';
import {AreaService} from '../../../services/expert/area.service';

@Component({
  selector: 'app-expert-work-place',
  templateUrl: './expert-work-place.component.html',
  styleUrls: ['./expert-work-place.component.css']
})
export class ExpertWorkPlaceComponent implements OnInit {
  public expertScoring: ExpertScoring[] = [];

  constructor(private expertApiClient: ExpertApiClient,
              private areaService: AreaService) { }

  ngOnInit() {
    this.expertApiClient.getMockExpertScoringAsync().subscribe(videos => this.expertScoring = videos);
  }
  public getAreaName(id) {
    return this.areaService.getAreaNameByIndex(id);
  }

  public estimateOffer(id: number, area: number): void {
    console.log(id, area);
  }

}
