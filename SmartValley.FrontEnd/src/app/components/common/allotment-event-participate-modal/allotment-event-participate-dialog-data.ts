import {Balance} from '../../../services/balance/balance';

export interface AllotmentEventParticipateDialogData {
    balance: Balance;
    myBet: number;
    totalBet: number;
    tokenBalance: number;
    decimals: number;
}
