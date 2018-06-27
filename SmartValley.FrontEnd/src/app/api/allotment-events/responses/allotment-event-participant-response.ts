import BigNumber from 'bignumber.js';

export interface AllotmentEventParticipantResponse {
  bid: BigNumber;
  share: BigNumber;
  userId: number;
  isCollected: boolean;
}
