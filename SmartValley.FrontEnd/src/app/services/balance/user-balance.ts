import {FrozenBalance} from './frozen-balance';
import BigNumber from 'bignumber.js';
import {Balance} from './balance';

export class UserBalance {

    public ethBalance: BigNumber;
    public wasEtherReceived: boolean;
    public svtBalance?: BigNumber;
    public frozenSVT?: FrozenBalance[];
    public totalFrozenSVT?: BigNumber;
    public svtDecimals?: number;
    public actualSVTbalance?: BigNumber;

    constructor(balance: Balance) {
        this.ethBalance = balance.ethBalance;
        this.wasEtherReceived = balance.wasEtherReceived;
        this.svtBalance = balance.svtBalance;
        this.frozenSVT = balance.frozenSVT;
        this.totalFrozenSVT = balance.totalFrozenSVT;
        this.svtDecimals = balance.svtDecimals;

        let frozenSVT = this.svtBalance;
        if (balance.frozenSVT) {
            frozenSVT = balance.frozenSVT.reduce((l, r) => l.plus(r.sum), new BigNumber(0));
            this.actualSVTbalance = this.svtBalance.minus(frozenSVT);
        }
    }
}
