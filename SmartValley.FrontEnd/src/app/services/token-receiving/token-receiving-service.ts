import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {AuthenticationService} from '../authentication/authentication-service';
import {TranslateService} from '@ngx-translate/core';
import {DialogService} from '../dialog-service';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class TokenService {

  private tokenContractAbi: string;
  private tokenContractAddress: string;

  private minterContractAbi: string;
  private minterContractAddress: string;

  private isInitialized: boolean;

  constructor(private dialogService: DialogService,
              private web3: Web3Service,
              private notificationsService: NotificationsService,
              private authenticationService: AuthenticationService,
              private contractClient: ContractApiClient,
              private translateService: TranslateService) {
  }

  private async initilizeAsync(): Promise<void> {

    const minterContract = await this.contractClient.getMinterContractAsync();
    this.minterContractAbi = minterContract.abi;
    this.minterContractAddress = minterContract.address;

    const tokenContract = await this.contractClient.getTokenContractAsync();
    this.tokenContractAbi = tokenContract.abi;
    this.tokenContractAddress = tokenContract.address;

    this.isInitialized = true;
  }

  private extractValueFromContractBoolResult(result: Array<any>): boolean {
    return result[0].toString(10);
  }

  private extractValueFromContractNumberResult(result: Array<any>): number {
    return +result[0].toString(10);
  }

  public async receiveAsync(): Promise<void> {
    const accountAddress = this.authenticationService.getCurrentUser().account;
    const token = this.web3.getContract(this.minterContractAbi, this.minterContractAddress);
    const transactionHash = await token.getTokens({from: accountAddress});

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
    const token = this.web3.getContract(this.minterContractAbi, this.minterContractAddress);
    return this.extractValueFromContractBoolResult(await token.canGetTokens(accountAddress));
  }

  async getBalanceAsync(accountAddress: string): Promise<number> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.tokenContractAbi, this.tokenContractAddress);
    const balance = this.extractValueFromContractNumberResult(await token.balanceOf(accountAddress));
    const decimals = this.extractValueFromContractNumberResult(await token.decimals());
    return this.web3.fromWei(balance, decimals);
  }
}
