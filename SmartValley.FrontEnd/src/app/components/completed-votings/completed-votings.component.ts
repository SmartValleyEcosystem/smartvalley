import {Component, OnInit} from '@angular/core';
import {VotingApiClient} from '../../api/voting/voting-api-client';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Voting} from '../../services/voting';

@Component({
  selector: 'app-completed-votings',
  templateUrl: './completed-votings.component.html',
  styleUrls: ['./completed-votings.component.css']
})
export class CompletedVotingsComponent implements OnInit {

  public votings: Array<Voting> = [];

  constructor(private router: Router,
              private votingClient: VotingApiClient) {
  }

  async ngOnInit() {
    const response = await this.votingClient.getСompletedSprintsAsync();
    if (response.items.length === 0) {
      await this.router.navigate([Paths.Root]);
    }

    for (const votingResponse of response.items) {
      this.votings.push(<Voting>{
        address: votingResponse.address,
        fromDate: votingResponse.startDate,
        endDate: votingResponse.endDate
      });
    }
  }

  showVoting(address: string) {
    this.router.navigate([Paths.CompletedVoting + '/' + address]);
  }
}
