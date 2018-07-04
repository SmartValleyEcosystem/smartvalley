import BigNumber from 'bignumber.js';

export interface AllotmentEventParticipateDialogData {
    existingUserBid: BigNumber;
    allotmentEventTotalBid: BigNumber;
    tokenBalance: BigNumber;
    tokenDecimals: number;
    svtDecimals: number;
    tokenTicker: string;
    actualSVTbalance?: BigNumber;
}
