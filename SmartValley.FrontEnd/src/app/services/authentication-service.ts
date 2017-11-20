import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';

import {User} from './user';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {Paths} from '../paths';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import 'rxjs/add/operator/map';
import {Subscription} from 'rxjs/Subscription';

@Injectable()
export class AuthenticationService {

  public accountChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private router: Router) {

    if (this.web3Service.isMetamaskInstalled && this.getUser() != null) {
      this.startBackgroundChecker();
    }
  }

  private readonly userKey = 'userKey';
  private backgroundChecker: Subscription;

  public isAuthenticated() {
    return !isNullOrUndefined(this.getUser());
  }

  public async authenticateAsync(): Promise<Boolean> {
    if (!this.web3Service.isMetamaskInstalled) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return;
    }
    const accounts = await this.web3Service.getAccountsAsync();
    const metamaskAccount = accounts[0];

    if (metamaskAccount == null) {
      this.notificationsService.warn('Account unavailable', 'Please unlock metamask');
      return;
    }

    const isRinkeby = await this.web3Service.checkRinkebyNetworkAsync();
    if (!isRinkeby) {
      this.notificationsService.warn('Wrong network', 'Please change to Rinkeby');
      return;
    }

    const user = this.getUser();
    if (!isNullOrUndefined(user)) {
      if (user.account !== metamaskAccount) {
        await this.handleAccountSwitchAsync(metamaskAccount);
        return true;
      }
      return await this.checkSignatureAsync(user.account, user.signature);
    }
    await this.signAndSaveAsync(metamaskAccount);
    return true;
  }

  private async handleAccountSwitchAsync(account: string) {
    const savedSignature = this.getSignatureByAddress(account);
    if (!isNullOrUndefined(savedSignature)) {
      const isSavedSignatureValid = await this.checkSignatureAsync(account, savedSignature);
      if (!isSavedSignatureValid) {
        await this.signAndSaveAsync(account);
      }
    } else {
      await this.signAndSaveAsync(account);
    }
  }

  private async signAndSaveAsync(account: string) {
    const signature = await this.web3Service.signAsync(account);
    this.saveUser(new User(account, signature));
    this.saveSignatureForAddrsess(account, signature);
  }

  private async checkSignatureAsync(account: string, signature: string): Promise<Boolean> {
    const recoveredSignature = await this.web3Service.recoverSignatureAsync(signature);
    return account === recoveredSignature;
  }

  public getUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveUser(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.accountChanged.emit(user);
    this.startBackgroundChecker();
  }

  private deleteUser() {
    localStorage.removeItem(this.userKey);
    this.accountChanged.emit();
  }

  private getSignatureByAddress(account: string) {
    return localStorage.getItem(account);
  }

  private saveSignatureForAddrsess(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  private removeSignatureByAddress(account: string) {
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
    const accounts = await this.web3Service.getAccountsAsync();
    const metamaskAccount = accounts[0];
    const user = this.getUser();
    if (isNullOrUndefined(user)) {
      this.stopUserSession();
    }
    if (user.account === metamaskAccount) {
      return;
    }
    const savedSignature = this.getSignatureByAddress(metamaskAccount);
    if (!isNullOrUndefined(savedSignature)) {
      const isSavedSignatureValid = await this.checkSignatureAsync(metamaskAccount, savedSignature);
      if (isSavedSignatureValid) {
        this.saveUser(new User(metamaskAccount, savedSignature));
      } else {
        this.stopUserSession();
      }
    } else {
      this.stopUserSession();
    }
  }

  private stopUserSession() {
    this.deleteUser();
    this.stopBackgroundChecker();
    this.router.navigate(['']);
  }
}
