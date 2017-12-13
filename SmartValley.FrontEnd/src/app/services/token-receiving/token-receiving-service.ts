import {Injectable} from '@angular/core';
import {TokenContractClient} from './token-contract-client';
import {MinterContractClient} from './minter-contract-client';

@Injectable()
export class TokenService {

  constructor(private tokenContractClient: TokenContractClient,
              private minterContractClient: MinterContractClient) {
  }

  public async receiveAsync(): Promise<void> {
    await this.minterContractClient.receiveAsync();
  }

  async canGetTokensAsync(accountAddress: string): Promise<boolean> {
    return await this.minterContractClient.canGetTokensAsync(accountAddress);
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    return await this.tokenContractClient.getBalanceAsync(accountAddress);
  }
}
