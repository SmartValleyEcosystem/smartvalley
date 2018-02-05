import {Injectable} from '@angular/core';
import {QuestionService} from '../questions/question-service';
import {MinterContractClient} from '../contract-clients/minter-contract-client';
import {TokenContractClient} from '../contract-clients/token-contract-client';
import {PromiseUtils} from '../../utils/promise-utils';
import {BalanceService} from '../balance/balance.service';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {VotingManagerContractClient} from '../contract-clients/voting-manager-contract-client';
import {VotingContractClient} from '../contract-clients/voting-contract-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {AdminContractClient} from '../contract-clients/admin-contract-client';

@Injectable()
export class InitializationService {

  public isAppInitialized: boolean;

  constructor(private questionService: QuestionService,
              private minterContractClient: MinterContractClient,
              private adminContractClient: AdminContractClient,
              private tokenContractClient: TokenContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private votingManagerContractClient: VotingManagerContractClient,
              private votingContractClient: VotingContractClient,
              private balanceService: BalanceService,
              private authenticationService: AuthenticationService) {
  }

  public async initializeAppAsync(): Promise<void> {
    if (this.isAppInitialized) {
      return;
    }

    await Promise.all([this.initializeAppInternalAsync(), this.waitAsync()]);
    this.isAppInitialized = true;
  }

  private async initializeAppInternalAsync(): Promise<void> {
    await Promise.all([
      this.authenticationService.initializeAsync(),
      this.questionService.initializeAsync(),
      this.minterContractClient.initializeAsync(),
      this.adminContractClient.initializeAsync(),
      this.tokenContractClient.initializeAsync(),
      this.scoringManagerContractClient.initializeAsync(),
      this.votingManagerContractClient.initializeAsync(),
      this.votingContractClient.initializeAsync(),
      this.balanceService.updateBalanceAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }
}
