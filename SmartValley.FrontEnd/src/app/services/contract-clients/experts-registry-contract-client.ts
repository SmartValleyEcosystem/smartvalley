import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';
import {AreaType} from '../../api/scoring/area-type.enum';

@Injectable()
export class ExpertsRegistryContractClient implements ContractClient {

  public abi: string;
  public address: string;

  constructor(private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private userContext: UserContext) {
  }

  public async initializeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getExpertRegistryContractAsync();
    this.abi = tokenContract.abi;
    this.address = tokenContract.address;
  }

  public applyAsync(areas: Array<AreaType>, applicationHash: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.apply(areas, applicationHash, {from: fromAddress});
  }

  public approveAsync(expert: string, areas: Array<AreaType>): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    try {
      return contract.approve(expert, areas, {from: fromAddress});
    } catch (e) {
      return null;
    }
  }

  public rejectAsync(expert: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    try {
      return contract.reject(expert, {from: fromAddress});
    } catch (e) {
      return null;
    }
  }

  public addAsync(expertAddress: string, expertiseAreas: number[]): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.add(expertAddress, expertiseAreas, {from: fromAddress});
  }

  public removeAsync(expertAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.remove(expertAddress, {from: fromAddress});
  }

  public enableAsync(expertAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.enable(expertAddress, {from: fromAddress});
  }

  public disableAsync(expertAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.disable(expertAddress, {from: fromAddress});
  }
}
