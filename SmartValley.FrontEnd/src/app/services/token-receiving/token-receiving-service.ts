import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {TranslateService} from '@ngx-translate/core';
import {DialogService} from '../dialog-service';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class TokenService {

  private contractAbi: string;
  private contractAddress: string;
  private isInitialized: boolean;

  constructor(private dialogService: DialogService,
              private http: HttpClient,
              private web3: Web3Service,
              private notificationsService: NotificationsService,
              private authenticationService: AuthenticationService,
              private contractClient: ContractApiClient,
              private translateService: TranslateService) {
  }

  private async initilizeAsync(): Promise<void> {
    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.contractAbi = tokenContract.abi;
    this.contractAddress = tokenContract.address;
    this.isInitialized = true;
  }

  private extractValueFromContractBoolResult(result: Array<any>): boolean {
    return result[0].toString(10);
  }

  private extractValueFromContractNumberResult(result: Array<any>): number {
    return +result[0].toString(10);
  }

  public async receiveAsync(): Promise<void> {
    const minterContract = await this.contractClient.getMinterContractAsync();
    const accountAddress = this.authenticationService.getCurrentUser().account;
    const transactionHash = await this.web3.eth.sendTransaction({
      to: minterContract.address,
      from: accountAddress,
      data: '0xaa6ca808'
    });

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('TokenReceiving.Dialog'),
      transactionHash
    );

    try {
      await this.web3.waitForConfirmationAsync(transactionHash);
      this.notificationsService.success(this.translateService.instant('TokenReceiving.Success'));
    } catch (e) {
      this.notificationsService.error(this.translateService.instant('TokenReceiving.Error'));
    }

    transactionDialog.close();
  }

  async canGetTokensAsync(accountAddress: string): Promise<boolean> {
    const minterContract = await this.contractClient.getMinterContractAsync();
    const token = this.web3.getContract(minterContract.abi, minterContract.address);
    return this.extractValueFromContractBoolResult(await token.canGetTokens(accountAddress));
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.contractAbi, this.contractAddress);
    const balance = this.extractValueFromContractNumberResult(await token.balanceOf(accountAddress));
    const decimals = this.extractValueFromContractNumberResult(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }
}
