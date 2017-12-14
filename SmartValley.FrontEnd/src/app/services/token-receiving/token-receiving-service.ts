import {Injectable} from '@angular/core';
import {TokenContractClient} from './token-contract-client';
import {MinterContractClient} from './minter-contract-client';
import {BalanceService} from '../balance/balance.service';

@Injectable()
export class TokenReceivingService {

  constructor(private tokenContractClient: TokenContractClient,
              private minterContractClient: MinterContractClient,
              private balanceService: BalanceService) {
  }

  public async receiveAsync(): Promise<void> {
    await this.minterContractClient.receiveAsync();
    await this.balanceService.updateBalanceAsync();
  }
}
