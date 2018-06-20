import {FrozenBalance} from './frozen-balance';

export interface Balance {
  ethBalance: number;
  wasEtherReceived: boolean;
  svt?: number;
  frozenSVT?: FrozenBalance[];
}
