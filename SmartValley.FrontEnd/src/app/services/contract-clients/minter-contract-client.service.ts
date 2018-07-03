import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {UserContext} from '../authentication/user-context';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {Initializable} from '../initializable';

@Injectable()
export class MinterContractClient implements Initializable {

  private abi: string;
  private address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getMinterContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
  }

  public async getTokensAsync(): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return contract.getTokens({from: fromAddress});
  }

  public async canGetTokensAsync(): Promise<boolean> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const fromAddress = this.userContext.getCurrentUser().account;

    return ConverterHelper.extractBoolValue(await contract.canGetTokens(fromAddress));
  }
}
