import {EventEmitter, Injectable} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {Balance} from './balance';
import {AuthenticationService} from '../authentication/authentication-service';
import {DialogService} from '../dialog-service';
import {Web3Service} from '../web3-service';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {UserContext} from '../authentication/user-context';
import BigNumber from 'bignumber.js';
import {SmartValleyTokenContractClient} from '../contract-clients/smart-valley-token-contract-client.service';

@Injectable()
export class BalanceService {

  public balanceChanged: EventEmitter<Balance> = new EventEmitter<Balance>();
  public balance: Balance;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private userContext: UserContext,
              private dialogService: DialogService,
              private notificationsService: NotificationsService,
              private web3Service: Web3Service,
              private translateService: TranslateService,
              private smartValleyTokenContractClient: SmartValleyTokenContractClient) {
    this.userContext.userContextChanged.subscribe(async () => await this.updateBalanceAsync());
  }

  public async updateBalanceAsync(): Promise<void> {
    if (!this.authenticationService.isAuthenticated()) {
      this.balanceChanged.emit(null);
      return;
    }
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    this.balance = {
      ethBalance: +balanceResponse.balance.toFixed(3),
      wasEtherReceived: balanceResponse.wasEtherReceived,
    };
    this.balanceChanged.emit(this.balance);
  }

  public async checkEthAsync(): Promise<boolean> {
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    if (balanceResponse.balance > 0.05) {
      return true;
    }
    if (!await this.dialogService.showGetEtherDialogAsync(balanceResponse.wasEtherReceived)) {
      return false;
    }
    return await this.receiveEtherAsync();
  }

  public async receiveEtherAsync(): Promise<boolean> {
    const receiveEtherResponse = await this.balanceApiClient.receiveEtherAsync();
    const transactionHash = receiveEtherResponse.transactionHash;
    return await this.showTransactionDialogAndGetResultAsync(transactionHash);
  }

  public async getTokenBalanceAsync(): Promise<Balance> {
    const svt = await this.smartValleyTokenContractClient.getBalanceAsync();
    const eth = await this.balanceApiClient.getBalanceAsync();
    const frozenBalances = await this.smartValleyTokenContractClient.getFreezingDetailsAsync();
    const decimals = await this.smartValleyTokenContractClient.getDecimalsAsync();
    const totalFrozenSVT = frozenBalances.map(b => b.sum).reduce((b1, b2) => b1.add(b2), new BigNumber(0));
    return await <Balance>{
      ethBalance: eth.balance,
      svt: svt,
      frozenSVT: frozenBalances,
      totalFrozenSVT: totalFrozenSVT,
      svtDecimals: decimals
    };
  }

  private async showTransactionDialogAndGetResultAsync(transactionHash: string): Promise<boolean> {
    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('Balance.TransactionDialog'),
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);
      this.notificationsService.success(this.translateService.instant('Balance.Success'));
    } catch (e) {
      this.notificationsService.error(this.translateService.instant('Balance.Error'));
      return false;
    }

    await this.updateBalanceAsync();
    transactionDialog.close();
    return true;
  }
}
