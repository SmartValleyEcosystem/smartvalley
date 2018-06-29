import BigNumber from 'bignumber.js';

export interface ReceiveTokensModalData {
  totalTokens: BigNumber;
  totalBet: BigNumber;
  userBet: BigNumber;
  userTokens: BigNumber;
  tokenTicker: string;
  tokenDecimals: number;
  svtDecimals: number;
}
