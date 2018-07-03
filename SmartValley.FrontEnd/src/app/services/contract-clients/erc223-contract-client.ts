import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import BigNumber from 'bignumber.js';
import {Initializable} from '../initializable';

@Injectable()
export class Erc223ContractClient implements Initializable {

  private abi: string;

  constructor(private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getERC223ContractAsync();
    this.abi = contractResponse.abi;
  }

  public async getSymbolAsync(tokenAddress: string): Promise<string> {
    const contract = this.web3Service.getContract(this.abi, tokenAddress);
    return ConverterHelper.extractStringValue(await contract.symbol());
  }

  public async getDecimalsAsync(tokenAddress: string): Promise<number> {
    const contract = this.web3Service.getContract(this.abi, tokenAddress);
    return ConverterHelper.extractNumberValue(await contract.decimals());
  }

  public async getTokenBalanceAsync(tokenAddress: string, holderAddress: string): Promise<BigNumber> {
    const contract = this.web3Service.getContract(this.abi, tokenAddress);
    return ConverterHelper.extractBigNumber(await contract.balanceOf(holderAddress));
  }
}
