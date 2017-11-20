import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';

import {User} from './user';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {Paths} from '../paths';

@Injectable()
export class AuthenticationService {


   public accountChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private router: Router) {
  }

  private readonly userKey = 'userKey';

  private getSignatureByAddress(account: string) {
    return localStorage.getItem(account);
  }

  private saveSignatureForAddrsess(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  private removeSignatureByAddress(account: string) {
    localStorage.removeItem(account);
  }

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
        this.accountChanged.emit(user);
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
  }


}
