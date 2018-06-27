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

  constructor(private allotmentEventsService: AllotmentEventService,
              private balanceService: BalanceService,
              private userContext: UserContext,
              private projectApiClient: ProjectApiClient) {
  }

  async ngOnInit() {
    await this.loadAllotmentEventsAsync();
    this.isAllotmentEventsLoaded = true;
    const tokenBalance = await this.balanceService.getTokenBalanceAsync();
    this.balance = tokenBalance;
    this.user = this.userContext.getCurrentUser();
    this.userContext.userContextChanged.subscribe((user) => this.user = user);
  }

  private async loadAllotmentEventsAsync(): Promise<void> {
    const events = await this.allotmentEventsService.getAllotmentEventsAsync(this.offset, this.pageSize, [AllotmentEventStatus.InProgress, AllotmentEventStatus.Finished]);
    this.allotmentEvents = events.map(i => new AllotmentEventCard(i));
    const projectIds = this.allotmentEvents.map(a => a.event.projectId);

    const projectResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: 0,
      count: projectIds.length,
      onlyScored: false,
      orderBy: ProjectsOrderBy.CreationDate,
      direction: SortDirection.Descending,
      projectIds: projectIds
    });

    this.allotmentEvents.map(a => a.project = projectResponse.items.find((p) => p.id === a.event.projectId));
    this.activeEvents = this.allotmentEvents.filter(a => a.event.status === AllotmentEventStatus.InProgress);
    this.finishedEvents = this.allotmentEvents.filter(a => a.event.status === AllotmentEventStatus.Finished);
  }

  public finishEvent(id: number) {
    this.finishedEvents.push(this.activeEvents.find((a) => a.event.id === id));
    this.activeEvents = this.activeEvents.filter((a) => {
      return a.event.id !== id;
    });
  }
}
