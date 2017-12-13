import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {AuthenticationService} from '../authentication/authentication-service';
import {DialogService} from '../dialog-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {TranslateService} from '@ngx-translate/core';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class MinterContractClient {

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

    this.isInitialized = true;
  }

  private extractBoolValue(result: Array<any>): boolean {
    return result[0];
  }

  public async receiveAsync(): Promise<void> {
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
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
    if (!this.isInitialized) {
      await this.initilizeAsync();
    }
    const token = this.web3.getContract(this.minterContractAbi, this.minterContractAddress);
    return this.extractBoolValue(await token.canGetTokens(accountAddress));
  }
}
