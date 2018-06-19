import {FrozenBalance} from './frozen-balance';

export interface TokenBalance {
    eth: number;
    svt: number;
    frozenSVT: FrozenBalance[];
}