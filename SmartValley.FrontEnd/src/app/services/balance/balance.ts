import {FrozenBalance} from './frozen-balance';
import BigNumber from 'bignumber.js';

export interface Balance {
  ethBalance: number;
  wasEtherReceived: boolean;
  svt?: BigNumber;
  frozenSVT?: FrozenBalance[];
  totalFrozenSVT?: BigNumber;
  svtDecimals?: number;
}
