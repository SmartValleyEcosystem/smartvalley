import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class Erc223ContractClient {

  public abi: string;

  constructor(private web3Service: Web3Service,
              private contractClient: ContractApiClient,
              private userContext: UserContext) {
  }

  public async getSymbolAsync(tokenAddress: string): Promise<string> {
    const tokenContract = await this.contractClient.getERC223ContractAsync();
    const contract = this.web3Service.getContract(tokenContract.abi, tokenAddress);
    return ConverterHelper.extractStringValue(await contract.symbol());
  }

  public async getDecimalsAsync(tokenAddress: string): Promise<number> {
    const tokenContract = await this.contractClient.getERC223ContractAsync();
    const contract = this.web3Service.getContract(tokenContract.abi, tokenAddress);
    return ConverterHelper.extractNumberValue(await contract.decimals());
  }

  public async getTokenBalanceAsync(tokenAddress: string, holderAddress: string): Promise<number> {
    const tokenContract = await this.contractClient.getERC223ContractAsync();
    const contract = this.web3Service.getContract(tokenContract.abi, tokenAddress);
    return ConverterHelper.extractNumberValue(await contract.balanceOf(holderAddress));
  }
}
