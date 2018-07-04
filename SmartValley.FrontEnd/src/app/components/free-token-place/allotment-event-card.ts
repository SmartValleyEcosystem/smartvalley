import {AllotmentEventTimer} from './allotment-event-timer';
import {ProjectResponse} from '../../api/project/project-response';
import {AllotmentEvent} from '../../services/allotment-event/allotment-event';
import {Balance} from '../../services/balance/balance';
import BigNumber from 'bignumber.js';

export class AllotmentEventCard {

  public timer: AllotmentEventTimer;
  public project?: ProjectResponse;
  public event: AllotmentEvent;
  public transaction?: string;
  public balance?: Balance;

  constructor(event: AllotmentEvent) {
    this.event = event;
    this.transaction = '';
    this.timer = <AllotmentEventTimer>{
      days: '00',
      hours: '00',
      minutes: '00',
      seconds: '00'
    };
  }
}
