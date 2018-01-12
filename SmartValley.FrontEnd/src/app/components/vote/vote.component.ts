import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {QuestionService} from '../../services/questions/question-service';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {Paths} from '../../paths';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {
  public details: ProjectDetailsResponse;
  public projectImageUrl: string;

  private projectId: number;

  public endDays: number;
  public endHours: number;
  public endMinutes: number;
  public endSeconds: number;

  constructor(private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute,
              private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService) {
  }


  public async ngOnInit() {
    await this.loadInitialData();
  }

  private async loadInitialData(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.details = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.blockiesService.getImageForAddress(this.details.projectAddress);
  }

  public async vote() {

  }

  public async navigateToVoting() {
    await this.router.navigate([Paths.Scoring]);
  }
}
