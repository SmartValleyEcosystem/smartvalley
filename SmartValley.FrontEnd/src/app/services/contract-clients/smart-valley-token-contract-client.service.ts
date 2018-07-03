import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {UserContext} from '../authentication/user-context';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import BigNumber from 'bignumber.js';
import {ConverterHelper} from '../converter-helper';
import {FrozenBalance} from '../balance/frozen-balance';
import {Initializable} from '../initializable';

@Injectable()
export class SmartValleyTokenContractClient implements Initializable {

  private abi: string;
  private address: string;

  constructor(private userContext: UserContext,
              private web3Service: Web3Service,
              private contractClient: ContractApiClient) {
  }

  public async initializeAsync(): Promise<void> {
    const contractResponse = await this.contractClient.getSmartValleyTokenContractAsync();
    this.abi = contractResponse.abi;
    this.address = contractResponse.address;
  }

  public async getBalanceAsync(): Promise<BigNumber> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const account = this.userContext.getCurrentUser().account;
    return ConverterHelper.extractBigNumber(await contract.balanceOf(account));
  }

  public async getDecimalsAsync(): Promise<number> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    return ConverterHelper.extractNumberValue(await contract.decimals());
  }

  public async getFreezingDetailsAsync(): Promise<FrozenBalance[]> {
    const contract = this.web3Service.getContract(this.abi, this.address);
    const account = this.userContext.getCurrentUser().account;
    const balances = await contract.getFreezingDetails(account);
    return balances[0].map((e, i) => <FrozenBalance>{
      date: new Date(+balances[1][i].toString() * 1000),
      sum: new BigNumber(e.toString())
    });
  }

  public async freezeAsync(tokensAmount: BigNumber, eventContractAddress: string): Promise<string> {
    const fromAddress = this.userContext.getCurrentUser().account;
    const contract = this.web3Service.getContract(this.abi, this.address);
    return await contract.freeze(tokensAmount, eventContractAddress, {from: fromAddress});
  }
}
