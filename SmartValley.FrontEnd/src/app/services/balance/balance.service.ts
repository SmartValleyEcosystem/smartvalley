import {EventEmitter, Injectable} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {Balance} from './balance';
import {AuthenticationService} from '../authentication/authentication-service';
import {TokenContractClient} from '../token-receiving/token-contract-client';
import {MinterContractClient} from '../token-receiving/minter-contract-client';

@Injectable()
export class BalanceService {

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private tokenContractClient: TokenContractClient,
              private minterContractClient: MinterContractClient) {
    this.authenticationService.accountChanged.subscribe(async () => await this.updateBalanceAsync());
  }

  public balanceChanged: EventEmitter<Balance> = new EventEmitter<Balance>();

  public async updateBalanceAsync(): Promise<void> {
    if (!this.authenticationService.isAuthenticated()) {
      this.balanceChanged.emit(null);
      return;
    }
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    const address = this.authenticationService.getCurrentUser().account;
    const svtBalance = await this.getSvtBalanceAsync(address);
    const canReceiveSvt = await this.canGetTokensAsync(address);

    const balance = {
      ethBalance: +balanceResponse.balance.toFixed(3),
      wasEtherReceived: balanceResponse.wasEtherReceived,
      svtBalance: svtBalance,
      canReceiveSvt: canReceiveSvt
    };
    this.balanceChanged.emit(balance);
  }

  private async canGetTokensAsync(accountAddress: string): Promise<boolean> {
    return await this.minterContractClient.canGetTokensAsync(accountAddress);
  }

  private async getSvtBalanceAsync(accountAddress: string): Promise<number> {
    return await this.tokenContractClient.getBalanceAsync(accountAddress);
  }
}
