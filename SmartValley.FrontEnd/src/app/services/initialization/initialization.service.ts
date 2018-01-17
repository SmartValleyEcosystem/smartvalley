import {Injectable} from '@angular/core';
import {QuestionService} from '../questions/question-service';
import {MinterContractClient} from '../contract-clients/minter-contract-client';
import {TokenContractClient} from '../contract-clients/token-contract-client';
import {PromiseUtils} from '../../utils/promise-utils';
import {BalanceService} from '../balance/balance.service';
import {ScoringContractClient} from '../contract-clients/scoring-contract-client';
import {ProjectManagerContractClient} from '../contract-clients/project-manager-contract-client';
import {VotingManagerContractClient} from '../contract-clients/voting-manager-contract-client';

@Injectable()
export class InitializationService {

  public isAppInitialized: boolean;

  constructor(private questionService: QuestionService,
              private minterContractClient: MinterContractClient,
              private tokenContractClient: TokenContractClient,
              private scoringContractClient: ScoringContractClient,
              private projectManagerContractClient: ProjectManagerContractClient,
              private votingManagerContractClient: VotingManagerContractClient,
              private balanceService: BalanceService) {
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
      this.questionService.initializeAsync(),
      this.minterContractClient.initializeAsync(),
      this.tokenContractClient.initializeAsync(),
      this.scoringContractClient.initializeAsync(),
      this.projectManagerContractClient.initializeAsync(),
      this.votingManagerContractClient.initializeAsync(),
      this.balanceService.updateBalanceAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }
}
