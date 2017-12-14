import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {AuthenticationService} from '../authentication/authentication-service';
import {DialogService} from '../dialog-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class TokenContractClient {

  private tokenContractAbi: string;
  private tokenContractAddress: string;

  private isInitialized: boolean;

  constructor(private dialogService: DialogService,
              private web3: Web3Service,
              private notificationsService: NotificationsService,
              private authenticationService: AuthenticationService,
              private contractClient: ContractApiClient) {
  }

  private async initilizeAsync(): Promise<void> {

    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.tokenContractAbi = tokenContract.abi;
    this.tokenContractAddress = tokenContract.address;

    this.isInitialized = true;
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.tokenContractAbi, this.tokenContractAddress);
    const balance = ConverterHelper.extractNumberValue(await token.balanceOf(accountAddress));
    const decimals = ConverterHelper.extractNumberValue(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }

  async getTokenDecimalsAsync(): Promise<number> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.tokenContractAbi, this.tokenContractAddress);
    return ConverterHelper.extractNumberValue(await token.decimals());
  }
}
