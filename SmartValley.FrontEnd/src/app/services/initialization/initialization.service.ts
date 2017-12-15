import {Injectable} from '@angular/core';
import {QuestionService} from '../questions/question-service';
import {MinterContractClient} from '../token-receiving/minter-contract-client';
import {TokenContractClient} from '../token-receiving/token-contract-client';
import {PromiseUtils} from '../../utils/promise-utils';
import {BalanceService} from '../balance/balance.service';
import {AuthenticationService} from '../authentication/authentication-service';

@Injectable()
export class InitializationService {

  constructor(private questionService: QuestionService,
              private minterContractClient: MinterContractClient,
              private tokenContractClient: TokenContractClient,
              private balanceService: BalanceService,
              private authenticationService: AuthenticationService) {
  }

  public isAppInitialized: boolean;

  public async initializeAppAsync(): Promise<void> {
    if (this.isAppInitialized) {
      return;
    }

    await Promise.all([this.initializeAppInternalAsync(), this.waitAsync()]);
    this.isAppInitialized = true;
  }

  private async initializeAppInternalAsync(): Promise<void> {
    await Promise.all([
      this.questionService.initializeAsync(),
      this.minterContractClient.initializeAsync(),
      this.tokenContractClient.initializeAsync(),
      this.balanceService.updateBalanceAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }

}
