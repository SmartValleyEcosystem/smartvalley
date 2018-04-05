import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {OffersApiClient} from '../../../api/expert/offers-api-client';
import {Area} from '../../../services/expert/area';
import {AreaService} from '../../../services/expert/area.service';
import {ScoringExpertsManagerContractClient} from '../../../services/contract-clients/scoring-experts-manager-contract-client';

@Component({
  selector: 'app-scoring-about',
  templateUrl: './scoring-about.component.html',
  styleUrls: ['./scoring-about.component.scss']
})
export class ScoringAboutComponent implements OnInit {

  public isProjectExists = false;
  public projectId: number;
  public area: Area;
  public project: ProjectSummaryResponse;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute,
              private areaService: AreaService,
              private scoringExpertsManagerContractClient: ScoringExpertsManagerContractClient,
              private offersApiClient: OffersApiClient) {
  }

  async ngOnInit() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    const areaType = +this.route.snapshot.paramMap.get('areaType');
    this.area = this.areaService.areas.find(i => i.areaType === areaType);
    this.project = await this.projectApiClient.getProjectSummaryAsync(this.projectId);
    if (this.project && this.area && this.project.scoringId) {
      this.isProjectExists = true;
    }
  }

  public async acceptOfferAsync() {
    const transactionHash = await this.scoringExpertsManagerContractClient.acceptOfferAsync(this.projectId.toString(), this.area.areaType);
    await this.offersApiClient.acceptExpertOfferAsync(transactionHash, this.project.scoringId, this.area.areaType);
  }

  public async declineOfferAsync() {
    const transactionHash = await this.scoringExpertsManagerContractClient.rejectOfferAsync(this.projectId.toString(), this.area.areaType);
    await this.offersApiClient.declineExpertOfferAsync(transactionHash, this.project.scoringId, this.area.areaType);
  }
}
