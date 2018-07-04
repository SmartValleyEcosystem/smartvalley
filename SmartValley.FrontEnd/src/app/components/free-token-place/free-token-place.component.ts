import {Component, OnInit} from '@angular/core';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {AllotmentEventCard} from './allotment-event-card';
import {ProjectQuery} from '../../api/project/project-query';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {SortDirection} from '../../api/sort-direction.enum';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {User} from '../../services/authentication/user';
import {UserContext} from '../../services/authentication/user-context';
import {AllotmentEventService} from '../../services/allotment-event/allotment-event.service';
import {AuthenticationService} from '../../services/authentication/authentication-service';

@Component({
  selector: 'app-free-token-place',
  templateUrl: './free-token-place.component.html',
  styleUrls: ['./free-token-place.component.scss']
})
export class FreeTokenPlaceComponent implements OnInit {

  public allotmentEvents: AllotmentEventCard[];
  public activeEvents: Array<AllotmentEventCard> = [];
  public finishedEvents: AllotmentEventCard[] = [];
  public offset = 0;
  public pageSize = 10;
  public showFrozenTooltip = false;
  public balance: Balance;
  public isAllotmentEventsLoaded = false;
  public user: User;
  public showReceiveTokensButton = false;

  constructor(private allotmentEventsService: AllotmentEventService,
              private balanceService: BalanceService,
              private userContext: UserContext,
              private authenticationService: AuthenticationService,
              private projectApiClient: ProjectApiClient) {
  }

  async ngOnInit() {
    this.balance = await this.balanceService.getTokenBalanceAsync();
    await this.loadAllotmentEventsAsync();
    this.isAllotmentEventsLoaded = true;
    this.userContext.userContextChanged.subscribe(async () => await this.loadAllotmentEventsAsync());
  }

  public finishEvent(id: number) {
    this.finishedEvents.push(this.activeEvents.find((a) => a.event.id === id));
    this.activeEvents = this.activeEvents.filter((a) => {
      return a.event.id !== id;
    });
  }

  private async loadAllotmentEventsAsync(): Promise<void> {
    const events = await this.allotmentEventsService.getAllotmentEventsAsync(this.offset,
      this.pageSize,
      [AllotmentEventStatus.InProgress, AllotmentEventStatus.Finished]);

    this.allotmentEvents = events.items.map(i => new AllotmentEventCard(i));
    const projectResponse = await this.loadProjectsAsync();

    this.allotmentEvents.map(a => {
      a.balance = this.balance;
      a.project = projectResponse.find((p) => p.id === a.event.projectId);
    });
    this.activeEvents = this.allotmentEvents.filter(a => a.event.status === AllotmentEventStatus.InProgress);
    this.finishedEvents = this.allotmentEvents.filter(a => a.event.status === AllotmentEventStatus.Finished);

    if (this.authenticationService.isAuthenticated()) {
      this.user = this.userContext.getCurrentUser();
      this.showReceiveTokensButton = await this.balanceService.canReceiveTokensAsync();
    }
  }

  private async loadProjectsAsync() {
    if (this.allotmentEvents.length === 0) {
      return [];
    }
    const projectIds = this.allotmentEvents.map(a => a.event.projectId);
    const projectsResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: 0,
      count: projectIds.length,
      onlyScored: false,
      orderBy: ProjectsOrderBy.CreationDate,
      direction: SortDirection.Descending,
      projectIds: projectIds
    });
    return projectsResponse.items;
  }

  public async receiveTokensAsync(): Promise<void> {
    const reciveTokens = await this.balanceService.receiveTokensAsync();
    if (reciveTokens) {
      this.showReceiveTokensButton = false;
      this.balance = await this.balanceService.getTokenBalanceAsync();
    }
  }
}
