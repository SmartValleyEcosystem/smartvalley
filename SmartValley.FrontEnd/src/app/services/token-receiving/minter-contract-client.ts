import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {AuthenticationService} from '../authentication/authentication-service';
import {DialogService} from '../dialog-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ConverterHelper} from '../converter-helper';
import {TranslateService} from '@ngx-translate/core';
import {NotificationsService} from 'angular2-notifications';

@Injectable()
export class MinterContractClient {

  private minterContractAbi: string;
  private minterContractAddress: string;

  constructor(private dialogService: DialogService,
              private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private authenticationService: AuthenticationService,
              private contractClient: ContractApiClient,
              private translateService: TranslateService) {
  }

  public async initializeAsync(): Promise<void> {
    const minterContract = await this.contractClient.getMinterContractAsync();
    this.minterContractAbi = minterContract.abi;
    this.minterContractAddress = minterContract.address;
  }

  public async receiveAsync(): Promise<void> {
    const accountAddress = this.authenticationService.getCurrentUser().account;
    const minter = this.web3Service.getContract(this.minterContractAbi, this.minterContractAddress);
    const transactionHash = await minter.getTokens({from: accountAddress});

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('TokenReceiving.Dialog'),
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);
      this.notificationsService.success(this.translateService.instant('TokenReceiving.Success'));
    } catch (e) {
      this.notificationsService.error(this.translateService.instant('TokenReceiving.Error'));
    }

    transactionDialog.close();
  }

  async canGetTokensAsync(accountAddress: string): Promise<boolean> {
    const token = this.web3Service.getContract(this.minterContractAbi, this.minterContractAddress);
    return ConverterHelper.extractBoolValue(await token.canGetTokens(accountAddress));
  }

  async getDaysInvervalBetweenReceiveAsync(): Promise<number> {
    const token = this.web3Service.getContract(this.minterContractAbi, this.minterContractAddress);
    return ConverterHelper.extractNumberValue(await token.DAYS_INTERVAL_BETWEEN_RECEIVE());
  }


  async getReceiveDateForAddressAsync(accountAddress: string): Promise<number> {
    const token = this.web3Service.getContract(this.minterContractAbi, this.minterContractAddress);
    return ConverterHelper.extractNumberValue(await token.receiversDateMap(accountAddress));
  }
}
