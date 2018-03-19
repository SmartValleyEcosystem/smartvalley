import {Injectable} from '@angular/core';
import {QuestionService} from '../questions/question-service';
import {PromiseUtils} from '../../utils/promise-utils';
import {BalanceService} from '../balance/balance.service';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {VotingManagerContractClient} from '../contract-clients/voting-manager-contract-client';
import {VotingContractClient} from '../contract-clients/voting-contract-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {AdminContractClient} from '../contract-clients/admin-contract-client';
import {ExpertsRegistryContractClient} from '../contract-clients/experts-registry-contract-client';
import {AreaService} from '../expert/area.service';

@Injectable()
export class InitializationService {

  public isAppInitialized: boolean;

  constructor(private questionService: QuestionService,
              private adminContractClient: AdminContractClient,
              private expertContractClient: ExpertsRegistryContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private votingManagerContractClient: VotingManagerContractClient,
              private votingContractClient: VotingContractClient,
              private balanceService: BalanceService,
              private authenticationService: AuthenticationService,
              private areaService: AreaService) {
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
      this.adminContractClient.initializeAsync(),
      this.scoringManagerContractClient.initializeAsync(),
      this.votingManagerContractClient.initializeAsync(),
      this.votingContractClient.initializeAsync(),
      this.balanceService.updateBalanceAsync(),
      this.expertContractClient.initializeAsync(),
      this.areaService.initializeAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }
}
