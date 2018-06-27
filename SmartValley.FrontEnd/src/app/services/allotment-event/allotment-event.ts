import {AllotmentEventResponse} from '../../api/allotment-events/responses/allotment-event-response';
import {isNullOrUndefined} from 'util';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {AllotmentEventParticipantResponse} from '../../api/allotment-events/responses/allotment-event-participant-response';
import BigNumber from 'bignumber.js';
import {AllotmentEventParticipant} from './allotment-event-participant';

export class AllotmentEvent {

  public id: number;
  public name: string;
  public isUpdating?: boolean;
  public status: AllotmentEventStatus;
  public tokenContractAddress: string;
  public eventContractAddress: string;
  public projectId: number;
  public startDate?: Date;
  public finishDate?: Date;
  public tokenDecimals: number;
  public tokenTicker: string;
  public participants: Array<AllotmentEventParticipant>;
  public totalTokens?: BigNumber;

  constructor(id: number,
              name: string,
              isUpdating: boolean | null,
              status: AllotmentEventStatus,
              tokenContractAddress: string,
              eventContractAddress: string,
              projectId: number,
              startDate: Date | null,
              finishDate: Date | null,
              tokenDecimals: number,
              tokenTicker: string,
              participants: Array<AllotmentEventParticipantResponse>) {
    this.id = id;
    this.name = name;
    this.isUpdating = isUpdating;
    this.status = status;
    this.tokenContractAddress = tokenContractAddress;
    this.eventContractAddress = eventContractAddress;
    this.projectId = projectId;
    this.startDate = startDate;
    this.finishDate = finishDate;
    this.tokenDecimals = tokenDecimals;
    this.tokenTicker = tokenTicker;
    this.participants = participants.map(i => AllotmentEventParticipant.create(i));
  }

  static create(response: AllotmentEventResponse): AllotmentEvent {
    return new AllotmentEvent(
      response.id,
      response.name,
      response.isUpdating,
      response.status,
      response.tokenContractAddress,
      response.eventContractAddress,
      response.projectId,
      response.startDate,
      response.finishDate,
      response.tokenDecimals,
      response.tokenTicker,
      response.participants);
  }

  public getPercentShare(userId: number) {
    if (!userId) {
        return new BigNumber(0);
    }
    const share = this.getUserBid(userId).mul(100).dividedBy(this.totalBid);
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getPotentialShare(svtBalance: BigNumber) {
    const share = svtBalance.dividedBy(svtBalance.plus(this.totalBid));
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getActualShare(userId: number) {
    if (!userId) {
      return new BigNumber(0);
    }
    const share = this.getUserBid(userId).dividedBy(this.totalBid);
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  get totalBid(): BigNumber {
    const total = new BigNumber(0);
    this.participants.map(i => total.plus(i.bid));
    return total;
  }

  public getUserTokens(userId: number, tokenBalance: BigNumber): BigNumber {
    if (isNullOrUndefined(tokenBalance)) {
      return new BigNumber(0);
    }
    return tokenBalance.dividedBy(this.totalBid).mul(this.getUserBid(userId));
  }

  public getUserBid(userId: number): BigNumber {
    const myBid = this.participants.find((a) => a.userId === userId);
    return myBid ? new BigNumber(myBid.bid) : new BigNumber(0);
  }

  public userHasBid(userId: number): boolean {
    return this.participants.some((a) => a.userId === userId);
  }

  public isCollected(userId: number): boolean {
    return !this.participants.some(i => i.userId === userId && i.isCollected)
  }
}
