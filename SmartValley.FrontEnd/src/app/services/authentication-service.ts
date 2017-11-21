import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {Paths} from '../paths';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import 'rxjs/add/operator/map';
import {Subscription} from 'rxjs/Subscription';

@Injectable()
export class AuthenticationService {

  public static MESSAGE_TO_SIGN = 'Confirm login';

  public accountChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private router: Router) {

    if (this.web3Service.isMetamaskInstalled && this.getCurrentUser() != null) {
      this.startBackgroundChecker();
    }
  }

  private readonly userKey = 'userKey';
  private backgroundChecker: Subscription;

  public isAuthenticated() {
    return !isNullOrUndefined(this.getCurrentUser());
  }

  public async authenticateAsync(): Promise<boolean> {
    if (!this.web3Service.isMetamaskInstalled) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return;
    }
    const accounts = await this.web3Service.getAccounts();
    const currentAccount = accounts[0];

    if (currentAccount == null) {
      this.notificationsService.warn('Account unavailable', 'Please unlock metamask');
      return;
    }

    const isRinkeby = await this.web3Service.checkRinkebyNetworkAsync();
    if (!isRinkeby) {
      this.notificationsService.warn('Wrong network', 'Please change to Rinkeby');
      return;
    }

    const shouldSign = await this.shouldSignAccount(currentAccount);
    if (shouldSign) {
      try {
        await this.signAndSaveAsync(currentAccount);
      }
      catch (e) {
        return false;
      }
    }
    return true;
  }

  private async shouldSignAccount(currentAccount: string): Promise<boolean> {
    const user = this.getCurrentUser();
    if (user == null) {
      return true;
    }

    if (user.account !== currentAccount) {
      const savedSignature = this.getSignatureByAccount(currentAccount);
      user.account = currentAccount;
      user.signature = savedSignature;
    }
    const isValid = await this.checkSignatureAsync(user.account, user.signature);
    return !isValid;
  }

  private async signAndSaveAsync(account: string): Promise<void> {
    const signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, account);
    this.saveCurrentUser({account, signature});
    this.saveSignatureForAccount(account, signature);
  }

  private async checkSignatureAsync(account: string, signature: string): Promise<boolean> {
    if (isNullOrUndefined(account) || isNullOrUndefined(signature)) {
      return false;
    }
    const recoveredSignature = await this.web3Service.recoverSignature(AuthenticationService.MESSAGE_TO_SIGN, signature);
    return account === recoveredSignature;
  }

  public getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveCurrentUser(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.accountChanged.emit(user);
    this.startBackgroundChecker();
  }

  private deleteCurrentUser() {
    localStorage.removeItem(this.userKey);
    this.accountChanged.emit();
  }

  private getSignatureByAccount(account: string) {
    return localStorage.getItem(account);
  }

  private saveSignatureForAccount(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  private removeSignatureByAccount(account: string) {
    localStorage.removeItem(account);
  }

  private startBackgroundChecker() {
    if (!isNullOrUndefined(this.backgroundChecker) && !this.backgroundChecker.closed) {
      return;
    }
    console.log('Starting background checker...');
    this.backgroundChecker = Observable.interval(5 * 1000)
      .map(async () => this.checkCurrentAuthStateAsync())
      .subscribe();
  }

  private stopBackgroundChecker() {
    if (!isNullOrUndefined(this.backgroundChecker) && !this.backgroundChecker.closed) {
      this.backgroundChecker.unsubscribe();
    }
  }

  private async checkCurrentAuthStateAsync() {
    const isMetamaskEnabled = this.web3Service.isMetamaskInstalled;
    if (!isMetamaskEnabled) {
      this.stopUserSession();
    }
    const accounts = await this.web3Service.getAccounts();
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

  private stopUserSession() {
    this.deleteCurrentUser()
    this.stopBackgroundChecker();
    this.router.navigate([Paths.Root]);
  }
}
