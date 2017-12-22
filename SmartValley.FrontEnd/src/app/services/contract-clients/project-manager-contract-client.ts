import {Injectable} from '@angular/core';
import {AuthenticationService} from '../authentication/authentication-service';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {TokenContractClient} from './token-contract-client';
import {ContractClient} from './contract-client';

@Injectable()
export class ProjectManagerContractClient implements ContractClient {

  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private tokenClient: TokenContractClient) {
  }

  public abi: string;
  public address: string;

  public async initializeAsync(): Promise<void> {

    const projectManagerContract = await this.contractClient.getProjectManagerContractAsync();
    this.abi = projectManagerContract.abi;
    this.address = projectManagerContract.address;
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
    const projectNamanger = this.web3Service.getContract(this.abi, this.address);
    const cost = ConverterHelper.extractNumberValue(await projectNamanger.projectCreationCostWEI());
    return this.web3Service.fromWei(cost, await this.tokenClient.getTokenDecimalsAsync());
  }
}
