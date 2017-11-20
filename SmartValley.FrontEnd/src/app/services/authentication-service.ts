import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';

import {User} from './user';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {Paths} from '../paths';

@Injectable()
export class AuthenticationService {


  // public userInfoChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service,
              private notificationsService: NotificationsService,
              private router: Router) {
  }

  private readonly userKey = 'userKey';

  public getSignatureByAddress(account: string) {
    return localStorage.getItem(account);
  }

  public saveSignatureForAddrsess(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  public removeSignatureByAddress(account: string) {
    localStorage.removeItem(account);
  }

  public async authenticate(): Promise<Boolean> {

    if (!this.web3Service.isMetamaskInstalled) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return;
    }
    const accounts = await this.web3Service.getAccounts();
    const metamaskAccount = accounts[0];

    if (metamaskAccount == null) {
      this.notificationsService.warn('Account unavailable', 'Please unlock metamask');
      return;
    }
    const isRinkeby = await this.web3Service.isRinkebyNetwork();

    if (!isRinkeby) {
      this.notificationsService.warn('Wrong network', 'Please change to Rinkeby');
      return;
    }

    const user = this.getUser();
    if (!isNullOrUndefined(user)) {
      if (user.account !== metamaskAccount) {
        await this.handleAccountSwitch(metamaskAccount);
        return true;
      }
      return await this.checkSignature(user.account, user.signature);
    }
    await this.signAndSave(metamaskAccount);
    return true;
  }

  private async handleAccountSwitch(account: string) {
    const savedSignature = this.getSignatureByAddress(account);
    if (!isNullOrUndefined(savedSignature)) {
      const isSavedSignatureValid = await this.checkSignature(account, savedSignature);
      if (!isSavedSignatureValid) {
        await this.signAndSave(account);
      }
    } else {
      await this.signAndSave(account);
    }
  }

  private async signAndSave(account: string) {
    const signature = await this.web3Service.sign(account);
    this.saveUser(new User(account, signature));
    this.saveSignatureForAddrsess(account, signature);
  }

  private async checkSignature(account: string, signature: string): Promise<Boolean> {
    const recoveredSignature = await this.web3Service.recoverSignature(signature);
    return account === recoveredSignature;
  }

  public getUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveUser(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }


}
