import BigNumber from 'bignumber.js';

export interface ReceiveTokensModalData {
  totalTokens: BigNumber;
  totalBid: BigNumber;
  userBid: BigNumber;
  userTokens: BigNumber;
  tokenTicker: string;
  tokenDecimals: number;
  svtDecimals: number;
}
