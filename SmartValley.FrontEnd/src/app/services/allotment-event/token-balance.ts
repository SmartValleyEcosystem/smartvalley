import BigNumber from 'bignumber.js';
import {TokenBalanceResponse} from '../../api/allotment-events/responses/token-balance-response';

export class TokenBalance {

  public tokenAddress: string;
  public holderAddress: string;
  public balance: BigNumber;

  constructor(tokenAddress: string,
              holderAddress: string,
              balance: string) {
    this.tokenAddress = tokenAddress;
    this.holderAddress = holderAddress;
    this.balance = new BigNumber(balance);
  }

  static create(response: TokenBalanceResponse): TokenBalance {
    return new TokenBalance(
      response.tokenAddress,
      response.holderAddress,
      response.balance);
  }
}
