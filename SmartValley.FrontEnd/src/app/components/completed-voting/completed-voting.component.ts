import {Component, OnInit} from '@angular/core';
import {VotingService} from '../../services/voting/voting-service';
import {VotingSprint} from '../../services/voting/voting-sprint';
import {BalanceService} from '../../services/balance/balance.service';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {Paths} from '../../paths';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-completed-voting',
  templateUrl: './completed-voting.component.html',
  styleUrls: ['./completed-voting.component.css']
})
export class CompletedVotingComponent implements OnInit {

  public ProjectCardType = ProjectCardType;
  public projects: Array<ProjectCardData> = [];
  public sprint: VotingSprint;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private balanceService: BalanceService,
              private sprintService: VotingService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadSprintAsync();
  }

  async navigateToCompletedVotings() {
    await this.router.navigate([Paths.CompletedVotings]);
  }

  private async loadSprintAsync() {
    const address = this.route.snapshot.paramMap.get('address');
    this.sprint = await this.sprintService.getSprintByAddressAsync(address);
    this.projects = this.sprint.projects.map(p => ProjectCardData.fromProject(p));
  }
}
