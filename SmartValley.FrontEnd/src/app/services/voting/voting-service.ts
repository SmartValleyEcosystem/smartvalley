import {Injectable} from '@angular/core';
import {Project} from '../project';
import {VotingSprint} from './voting-sprint';
import {VotingApiClient} from '../../api/voting/voting-api-client';
import * as moment from 'moment';

@Injectable()
export class VotingService {

  constructor(private votingApiClient: VotingApiClient) {

  }

  public async hasActiveSprintAsync(): Promise<boolean> {
    const currentSprint = await this.getCurrentSprintAsync();
    return !!currentSprint;
  }

  public async getCurrentSprintAsync(): Promise<VotingSprint> {
    const lastSprintResponse = await this.votingApiClient.getLastVotingSprintAsync();
    if (!lastSprintResponse.doesExist) {
      return null;
    }

    const projects = [];
    for (const projectVoteResponse of lastSprintResponse.lastSprint.projects) {
      projects.push(<Project>{
        id: projectVoteResponse.id,
        name: projectVoteResponse.name,
        area: projectVoteResponse.area,
        country: projectVoteResponse.country,
        description: projectVoteResponse.description,
        address: projectVoteResponse.author,
        isVotedByMe: projectVoteResponse.isVotedByMe
      });
    }

    return <VotingSprint> {
      projects: projects,
      voteBalance: lastSprintResponse.lastSprint.voteBalance,
      startDate: moment(lastSprintResponse.lastSprint.startDate).toDate(),
      endDate: moment(lastSprintResponse.lastSprint.endDate).toDate()
    };
  }
}
