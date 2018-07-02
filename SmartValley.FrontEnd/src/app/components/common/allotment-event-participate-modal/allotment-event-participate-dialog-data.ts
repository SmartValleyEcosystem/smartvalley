import BigNumber from 'bignumber.js';

export interface AllotmentEventParticipateDialogData {
    userSvtBalance?: BigNumber;
    existingUserBid: BigNumber;
    allotmentEventTotalBid: BigNumber;
    tokenBalance: BigNumber;
    tokenDecimals: number;
    svtDecimals: number;
    tokenTicker: string;
}
