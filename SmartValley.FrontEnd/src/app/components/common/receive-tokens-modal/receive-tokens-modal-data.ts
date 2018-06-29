import BigNumber from 'bignumber.js';

export interface ReceiveTokensModalData {
  totalTokens: BigNumber;
  totalBet: BigNumber;
  userBet: BigNumber;
  userTokens: number;
  tokenTicker: string;
  tokenDecimals: number;
  svtDecimals: number;
}
