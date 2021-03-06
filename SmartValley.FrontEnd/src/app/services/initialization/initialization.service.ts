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
import BigNumber from 'bignumber.js';
import {MinterContractClient} from '../contract-clients/minter-contract-client.service';
import {Erc223ContractClient} from '../contract-clients/erc223-contract-client';
import {AllotmentEventContractClient} from '../contract-clients/allotment-event-contract-client.service';

@Injectable()
export class InitializationService {

  public isAppInitialized: boolean;

  constructor(private scoringCriterionService: ScoringCriterionService,
              private adminContractClient: AdminContractClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private minterContractClient: MinterContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private privateScoringManagerContractClient: PrivateScoringManagerContractClient,
              private scoringOffersManagerContractClient: ScoringOffersManagerContractClient,
              private scoringParametersProviderContractClient: ScoringParametersProviderContractClient,
              private authenticationService: AuthenticationService,
              private allotmentEventsManagerContractClient: AllotmentEventsManagerContractClient,
              private smartValleyTokenContractClient: SmartValleyTokenContractClient,
              private erc223ContractClient: Erc223ContractClient,
              private allotmentEventContractClient: AllotmentEventContractClient,
              private areaService: AreaService,
              private dictionariesService: DictionariesService) {
  }

  public async initializeAppAsync(): Promise<void> {
    if (this.isAppInitialized) {
      return;
    }
    BigNumber.config({
      DECIMAL_PLACES: 100,
      FORMAT: {
        decimalSeparator: '.',
        groupSeparator: ' ',
        groupSize: 3,
        secondaryGroupSize: 3,
        fractionGroupSeparator: ' ',
        fractionGroupSize: 3
      }
    });
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
      this.expertsRegistryContractClient.initializeAsync(),
      this.smartValleyTokenContractClient.initializeAsync(),
      this.minterContractClient.initializeAsync(),
      this.allotmentEventContractClient.initializeAsync(),
      this.areaService.initializeAsync(),
      this.dictionariesService.initializeAsync(),
      this.erc223ContractClient.initializeAsync()
    ]);
  }

  private async waitAsync(): Promise<void> {
    await PromiseUtils.delay(1 * 1000);
  }
}
