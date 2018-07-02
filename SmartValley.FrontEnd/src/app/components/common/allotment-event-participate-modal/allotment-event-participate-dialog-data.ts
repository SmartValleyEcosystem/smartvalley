import {Balance} from '../../../services/balance/balance';
import BigNumber from 'bignumber.js';

export interface AllotmentEventParticipateDialogData {
    balance: Balance;
    myBet: BigNumber;
    totalBet: BigNumber;
    tokenBalance: BigNumber;
    decimals: number;
    ticker: string;
    svtDecimals: number;
}
