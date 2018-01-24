import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {Paths} from '../../paths';
import {VotingSprint} from '../../services/voting/voting-sprint';
import {VotingService} from '../../services/voting/voting-service';
import {VotingContractClient} from '../../services/contract-clients/voting-contract-client';
import {DialogService} from '../../services/dialog-service';

@Component({
  selector: 'app-voting-card',
  templateUrl: './voting-card.component.html',
  styleUrls: ['./voting-card.component.css']
})
export class VotingCardComponent implements OnInit {
  public projectDetails: ProjectDetailsResponse;
  public projectImageUrl: string;

  private projectId: number;
  public currentSprint: VotingSprint;

  public endDays: number;
  public endHours: number;
  public endMinutes: number;
  public endSeconds: number;

  public isVotedByMe: boolean;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute,
              private router: Router,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private votingService: VotingService) {
  }

  public async ngOnInit() {
    await this.loadInitialData();
    await this.loadSprintAsync();
    this.updateDate();
    setInterval(() => this.updateDate(), 1000);
  }

  private updateDate(): void {
    const currentDate = new Date();
    this.endDays = this.currentSprint.endDate.getDate() - currentDate.getDate();
    if (this.endDays < 0) {
      this.endDays = new Date(currentDate.getFullYear(), currentDate.getMonth(), 0).getDate() + this.endDays;
    }
    this.endHours = this.currentSprint.endDate.getHours() - currentDate.getHours();
    this.endMinutes = this.currentSprint.endDate.getMinutes() - currentDate.getMinutes();
    this.endSeconds = this.currentSprint.endDate.getSeconds() - currentDate.getSeconds();
  }

  private async loadInitialData(): Promise<void> {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.projectDetails = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.projectImageUrl = this.getImageUrl();
  }

  private getImageUrl(): string {
    const address = this.projectDetails.projectAddress ? this.projectDetails.projectAddress : this.projectDetails.authorAddress;
    return this.blockiesService.getImageForAddress(address);
  }

  private async loadSprintAsync() {
    this.currentSprint = await this.votingService.getCurrentSprintAsync();
    this.isVotedByMe = this.currentSprint.projects.find(p => p.externalId === this.projectDetails.externalId).isVotedByMe;
  }

  public async voteAsync(): Promise<void> {
    await this.votingService.submitVoteAsync(
      this.currentSprint.address,
      this.projectDetails.externalId,
      this.projectDetails.name,
      this.currentSprint.voteBalance,
      this.currentSprint.endDate);
  }

  public async navigateToVoting() {
    await this.router.navigate([Paths.Voting]);
  }
}
