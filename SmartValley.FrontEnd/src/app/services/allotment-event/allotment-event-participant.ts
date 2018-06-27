import BigNumber from 'bignumber.js';
import {AllotmentEventParticipantResponse} from '../../api/allotment-events/responses/allotment-event-participant-response';

export class AllotmentEventParticipant {

  public bid: BigNumber;
  public share: BigNumber;
  public userId: number;
  public isCollected: boolean;

  constructor(bid: string,
              share: string,
              userId: number,
              isCollected: boolean) {

    this.bid = new BigNumber(bid);
    this.share = new BigNumber(share);
    this.userId = userId;
    this.isCollected = isCollected;
  }

  static create(response: AllotmentEventParticipantResponse): AllotmentEventParticipant {
    return new AllotmentEventParticipant(
      response.bid,
      response.share,
      response.userId,
      response.isCollected
    );
  }
}
