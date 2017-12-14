import {Injectable} from '@angular/core';
import {AuthenticationService} from './authentication/authentication-service';
import {Web3Service} from './web3-service';
import {ContractApiClient} from '../api/contract/contract-api-client';
import {ConverterHelper} from './converter-helper';
import {TokenContractClient} from './token-receiving/token-contract-client';

@Injectable()
export class ProjectManagerContractClient {

  private projectManagerContractAbi: string;
  private projectManagerAddress: string;

  private isInitialized: boolean;

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private tokenClient: TokenContractClient) {
  }

  private async initilizeAsync(): Promise<void> {

    const projectManagerContract = await this.contractClient.getProjectManagerContractAsync();
    this.projectManagerContractAbi = projectManagerContract.abi;
    this.projectManagerAddress = projectManagerContract.address;

    this.isInitialized = true;
  }

  public addProjectAsync(contractAddress: string,
                         abiString: string,
                         projectId: string,
                         name: string): Promise<string> {
    const projectContract = this.web3Service.getContract(abiString, contractAddress);
    const fromAddress = this.authenticationService.getCurrentUser().account;

    return projectContract.addProject(
      projectId.replace(/-/g, ''),
      name,
      {from: fromAddress});
  }

  public async getProjectCreationCostAsync() {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }

    const projectNamanger = this.web3Service.getContract(this.projectManagerContractAbi, this.projectManagerAddress);
    const cost = ConverterHelper.extractNumberValue(await projectNamanger.projectCreationCostWEI());
    return this.web3Service.fromWei(cost, await this.tokenClient.getTokenDecimalsAsync());
  }
}
