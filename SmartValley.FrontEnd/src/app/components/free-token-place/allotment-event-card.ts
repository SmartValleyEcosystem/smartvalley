import {AllotmentEventTimer} from './allotment-event-timer';
import {ProjectResponse} from '../../api/project/project-response';
import {AllotmentEvent} from '../../services/allotment-event/allotment-event';

export class AllotmentEventCard {

  public timer?: AllotmentEventTimer;
  public project?: ProjectResponse;
  public event: AllotmentEvent;
  public transaction?: string;

  constructor(event: AllotmentEvent) {
    this.event = event;
  }
}
