import {EventEmitter, Injectable} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {Balance} from './balance';
import {AuthenticationService} from '../authentication/authentication-service';
import {TokenContractClient} from '../contract-clients/token-contract-client';
import {MinterContractClient} from '../contract-clients/minter-contract-client';
import {ScoringManagerContractClient} from '../contract-clients/scoring-manager-contract-client';
import {DialogService} from '../dialog-service';
import {Web3Service} from '../web3-service';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class BalanceService {

  public balanceChanged: EventEmitter<Balance> = new EventEmitter<Balance>();
  public balance: Balance;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private tokenContractClient: TokenContractClient,
              private minterContractClient: MinterContractClient,
              private scoringManagerContractClient: ScoringManagerContractClient,
              private dialogService: DialogService,
              private notificationsService: NotificationsService,
              private web3Service: Web3Service,
              private translateService: TranslateService) {
    this.authenticationService.accountChanged.subscribe(async () => await this.updateBalanceAsync());
  }

  public async updateBalanceAsync(): Promise<void> {
    if (!this.authenticationService.isAuthenticated()) {
      this.balanceChanged.emit(null);
      return;
    }
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    const address = this.authenticationService.getCurrentUser().account;
    const svtBalance = await this.tokenContractClient.getBalanceAsync(address);
    const canReceiveSvt = await this.minterContractClient.canGetTokensAsync(address);
    this.balance = {
      votingBalance: 250,
      ethBalance: +balanceResponse.balance.toFixed(3),
      wasEtherReceived: balanceResponse.wasEtherReceived,
      svtBalance: +svtBalance.toFixed(3),
      canReceiveSvt: canReceiveSvt
    };
    this.balanceChanged.emit(this.balance);
  }

  public async checkEthAsync(): Promise<boolean> {
    const userHasETH = await this.hasUserEthAsync();
    if (userHasETH) {
      return true;
    }
    const wasEtherReceived = await this.wasEtherReceivedAsync();
    if (wasEtherReceived) {
      return await this.dialogService.showRinkeByDialogAsync();
    }
    if (await this.dialogService.showGetEtherDialogAsync()) {
      return await this.receiveEtherAsync();
    }
    return false;
  }

  public async checkSvtForProjectAsync(): Promise<boolean> {
    const userHasSvt = await this.hasUserSvtForProjectAsync();
    if (userHasSvt) {
      return true;
    }
    const dateToReceive = await this.getDateToReceiveTokensAsync();
    if (dateToReceive.getTime() > Date.now()) {
      return await this.dialogService.showSvtDialogAsync(dateToReceive.toLocaleDateString());
    }
    if (await this.dialogService.showGetTokenDialogAsync()) {
      return await this.receiveSvtAsync();
    }
    return false;
  }

  public async receiveEtherAsync(): Promise<boolean> {
    const transactionHash = (await this.balanceApiClient.receiveEtherAsync()).transactionHash;
    return await this.showTransactionDialogAndGetResultAsync(transactionHash);
  }

  public async receiveSvtAsync(): Promise<boolean> {
    const transactionHash = await this.minterContractClient.getTokensAsync();
    return await this.showTransactionDialogAndGetResultAsync(transactionHash);
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

  private async hasUserEthAsync(): Promise<boolean> {
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    return balanceResponse.balance > 0.05;
  }

  private async wasEtherReceivedAsync(): Promise<boolean> {
    const balanceResponse = await this.balanceApiClient.getBalanceAsync();
    return balanceResponse.wasEtherReceived;
  }

  private async hasUserSvtForProjectAsync(): Promise<boolean> {
    const accountAddress = (await this.authenticationService.getCurrentUser()).account;
    const tokenBalance = await this.tokenContractClient.getBalanceAsync(accountAddress);
    const scoringCost = await this.scoringManagerContractClient.getScoringCostAsync();
    return tokenBalance >= scoringCost;
  }

  private async getDateToReceiveTokensAsync(): Promise<Date> {
    const accountAddress = (await this.authenticationService.getCurrentUser()).account;
    const getReceiveDateForAddress = await this.minterContractClient.getReceiveDateForAddressAsync(accountAddress);
    const daysToReceive = await this.minterContractClient.getReceivingIntervalInDaysAsync();
    const dateToReceive = new Date(getReceiveDateForAddress * 1000);
    dateToReceive.setDate(dateToReceive.getDate() + daysToReceive);
    return dateToReceive;
  }
}
