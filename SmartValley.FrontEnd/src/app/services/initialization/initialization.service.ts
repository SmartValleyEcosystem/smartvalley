import {Injectable} from '@angular/core';
import {ScoringCriterionService} from '../criteria/scoring-criterion.service';
import {PromiseUtils} from '../../utils/promise-utils';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {AdminContractClient} from '../contract-clients/admin-contract-client';
import {ExpertsRegistryContractClient} from '../contract-clients/experts-registry-contract-client';
import {AreaService} from '../expert/area.service';
import {ScoringOffersManagerContractClient} from '../contract-clients/scoring-offers-manager-contract-client.service';
import {DictionariesService} from '../common/dictionaries.service';
import {PrivateScoringManagerContractClient} from '../contract-clients/private-scoring-manager-contract-client';
import {ScoringParametersProviderContractClient} from '../contract-clients/scoring-parameters-provider-contract-client';
import {AllotmentEventsManagerContractClient} from '../contract-clients/allotment-events-manager-contract-client';
import {SmartValleyTokenContractClient} from '../contract-clients/smart-valley-token-contract-client.service';

@Injectable()
export class InitializationService {

  public isAppInitialized: boolean;

  constructor(private scoringCriterionService: ScoringCriterionService,
              private adminContractClient: AdminContractClient,
              private expertContractClient: ExpertsRegistryContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private privateScoringManagerContractClient: PrivateScoringManagerContractClient,
              private scoringOffersManagerContractClient: ScoringOffersManagerContractClient,
              private scoringParametersProviderContractClient: ScoringParametersProviderContractClient,
              private authenticationService: AuthenticationService,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private smartValleyTokenContractClient: SmartValleyTokenContractClient,
              private areaService: AreaService,
              private dictionariesService: DictionariesService) {
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
      this.scoringParametersProviderContractClient.initializeAsync(),
      this.allotmentEventsManagerContractClient.initializeAsync(),
      this.authenticationService.initializeAsync(),
      this.scoringCriterionService.initializeAsync(),
      this.adminContractClient.initializeAsync(),
      this.scoringManagerContractClient.initializeAsync(),
      this.privateScoringManagerContractClient.initializeAsync(),
      this.scoringOffersManagerContractClient.initializeAsync(),
      this.expertContractClient.initializeAsync(),
      this.smartValleyTokenContractClient.initializeAsync(),
      this.areaService.initializeAsync(),
      this.dictionariesService.initializeAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }
}
