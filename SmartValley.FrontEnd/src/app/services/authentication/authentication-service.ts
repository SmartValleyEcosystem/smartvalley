import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from '../web3-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import 'rxjs/add/operator/map';
import {Ng2DeviceService} from 'ng2-device-detector';
import {Constants} from '../../constants';
import {DialogService} from '../dialog-service';

@Injectable()
export class AuthenticationService {
  public static MESSAGE_TO_SIGN = 'Confirm login. Please press the \'Sign\' button below.';
  public accountChanged: EventEmitter<any> = new EventEmitter<any>();

  private readonly userKey = 'userKey';
  private backgroundChecker: Subscription;
  private readonly compatibleBrowsers = [Constants.Chrome, Constants.Firefox];

  constructor(private web3Service: Web3Service,
              private router: Router,
              private deviceService: Ng2DeviceService,
              private dialogService: DialogService) {
    if (this.web3Service.isMetamaskInstalled && this.getCurrentUser() != null) {
      this.startBackgroundChecker();
    }
  }

  private getSignatureByAccount(account: string): string {
    return localStorage.getItem(account);
  }

  private saveSignatureForAccount(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  public isAuthenticated() {
    return !isNullOrUndefined(this.getCurrentUser());
  }

  public async authenticateAsync(): Promise<boolean> {
    if (!this.compatibleBrowsers.includes(this.deviceService.browser)) {
      this.dialogService.showIncompatibleBrowserAlert();
      return false;
    }

    if (!this.web3Service.isMetamaskInstalled) {
      await this.router.navigate([Paths.MetaMaskHowTo]);
      return false;
    }
    const accounts = await this.web3Service.getAccountsAsync();
    const currentAccount = accounts[0];

    if (currentAccount == null) {
      this.dialogService.showUnlockAccountAlert();
      return false;
    }

    const isRinkeby = await this.web3Service.checkRinkebyNetworkAsync();

    if (!isRinkeby) {
      this.dialogService.showRinkebyAlert();
      return false;
    }

    let signature = this.getSignatureByAccount(currentAccount);
    const shouldSign = await this.shouldSignAccount(currentAccount, signature);
    if (shouldSign) {
      try {
        signature = await this.web3Service.signAsync(AuthenticationService.MESSAGE_TO_SIGN, currentAccount);
      } catch (e) {
        return false;
      }
    }
    this.startUserSession(currentAccount, signature);
    return true;
  }

  private async shouldSignAccount(currentAccount: string, savedSignature: string): Promise<boolean> {
    let user = this.getCurrentUser();
    if (user == null || user.account !== currentAccount) {
      user = {account: currentAccount, signature: savedSignature};
    }
    return !await this.checkSignatureAsync(user.account, user.signature);
  }

  private startUserSession(account: string, signature: string) {
    const user = this.getCurrentUser();
    if (user != null && user.account === account && user.signature === signature) {
      return;
    }
    this.saveCurrentUser({account, signature});
    this.saveSignatureForAccount(account, signature);
    this.startBackgroundChecker();
  }

  private async checkSignatureAsync(account: string, signature: string): Promise<boolean> {
    if (isNullOrUndefined(account) || isNullOrUndefined(signature)) {
      return false;
    }
    try {
      const recoveredSignature = await this.web3Service.recoverSignature(AuthenticationService.MESSAGE_TO_SIGN, signature);
      return account === recoveredSignature;
    } catch (e) {
      return false;
    }
  }

  public getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveCurrentUser(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.accountChanged.emit(user);
  }

  private deleteCurrentUser() {
    localStorage.removeItem(this.userKey);
    this.accountChanged.emit();
  }

  private startBackgroundChecker() {
    if (!isNullOrUndefined(this.backgroundChecker) && !this.backgroundChecker.closed) {
      return;
    }
    this.backgroundChecker = Observable.interval(3 * 1000)
      .map(async () => this.checkCurrentAuthStateAsync())
      .subscribe();
  }

  private stopBackgroundChecker() {
    if (!isNullOrUndefined(this.backgroundChecker) && !this.backgroundChecker.closed) {
      this.backgroundChecker.unsubscribe();
    }
  }

  private async checkCurrentAuthStateAsync(): Promise<void> {
    const accounts = await this.web3Service.getAccountsAsync();
    const currentAccount = accounts[0];
    const user = this.getCurrentUser();
    if (isNullOrUndefined(user)) {
      this.stopUserSession();
    }
    if (user.account === currentAccount) {
      return;
    }
    const savedSignature = this.getSignatureByAccount(currentAccount);
    const isSavedSignatureValid = await this.checkSignatureAsync(currentAccount, savedSignature);
    if (isSavedSignatureValid) {
      this.saveCurrentUser({account: currentAccount, signature: savedSignature});
    } else {
      this.stopUserSession();
    }
  }

  public stopUserSession() {
    this.deleteCurrentUser();
    this.stopBackgroundChecker();
    this.router.navigate([Paths.Root]);
  }
}
