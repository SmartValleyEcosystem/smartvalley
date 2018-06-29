import {AllotmentEventResponse} from '../../api/allotment-events/responses/allotment-event-response';
import {isNullOrUndefined} from 'util';
import {AllotmentEventStatus} from '../../api/allotment-events/allotment-event-status';
import {AllotmentEventParticipantResponse} from '../../api/allotment-events/responses/allotment-event-participant-response';
import BigNumber from 'bignumber.js';
import {AllotmentEventParticipant} from './allotment-event-participant';

export class AllotmentEvent {

  public id: number;
  public name: string;
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

  public totalBid = new BigNumber(0);

  constructor(id: number,
              name: string,
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
    this.status = status;
    this.tokenContractAddress = tokenContractAddress;
    this.eventContractAddress = eventContractAddress;
    this.projectId = projectId;
    this.startDate = startDate;
    this.finishDate = finishDate;
    this.tokenDecimals = tokenDecimals;
    this.tokenTicker = tokenTicker;
    this.participants = participants.map(i => AllotmentEventParticipant.create(i));

    this.participants.map(i => this.totalBid = this.totalBid.plus(i.bid));
  }

  static create(response: AllotmentEventResponse): AllotmentEvent {
    return new AllotmentEvent(
      response.id,
      response.name,
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
    if (this.totalBid.isZero()) {
      return new BigNumber(0);
    }
    const share = this.getUserBid(userId).mul(100).dividedBy(this.totalBid);
    if (share.isNaN()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getPotentialShare(svtBalance: BigNumber) {
    if (svtBalance.isZero()) {
      return new BigNumber(0);
    }
    const share = svtBalance.dividedBy(svtBalance.plus(this.totalBid));
    if (share.isNaN() || !share.isFinite()) {
      return new BigNumber(0);
    }
    return share;
  }

  public getActualShare(userId: number) {
    if (!userId) {
      return new BigNumber(0);
    }
    const share = this.getUserBid(userId).dividedBy(this.totalBid);
    if (share.isNaN() || !share.isFinite()) {
      return new BigNumber(0);
    }
    return this.totalTokens.mul(share);
  }

  public getUserTokens(userId: number, tokenBalance: BigNumber): BigNumber {
    if (isNullOrUndefined(tokenBalance)) {
      return new BigNumber(0);
    }
    if (this.getUserBid(userId) <= new BigNumber(0)) {
      return tokenBalance;
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
    return this.participants.some(i => i.userId === userId && i.isCollected);
  }
}
