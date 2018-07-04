import {FrozenBalance} from './frozen-balance';
import BigNumber from 'bignumber.js';

export interface Balance {
  ethBalance: BigNumber;
  wasEtherReceived: boolean;
  svtBalance?: BigNumber;
  frozenSVT?: FrozenBalance[];
  totalFrozenSVT?: BigNumber;
  svtDecimals?: number;
}
