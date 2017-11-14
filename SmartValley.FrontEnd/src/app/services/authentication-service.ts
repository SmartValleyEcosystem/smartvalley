import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';
import {Resolve} from '@angular/router';
import {NotificationService} from './notification-service';
import {UserInfo} from './user-info';
import {Observable} from 'rxjs/Observable';
import {Paths} from '../paths';


@Injectable()
export class AuthenticationService {

  constructor(private web3Service: Web3Service, private notificationService: NotificationService) {
  }

  static MESSAGE_TO_SIGN = 'Confirm login';

  public getSignatureByAddress(account: string) {
    return window.localStorage[account];
  }

  public saveSignatureByAddrsess(account: string, signature: string) {
    window.localStorage[account] = signature;
  }

  public remove(account: string) {
    window.localStorage.removeItem(account);
  }

  public async authenticate(): Promise<Boolean> {
    try {
      const accounts = await this.web3Service.getAccounts();
      const signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, accounts[0]);
      this.saveSignatureByAddrsess(accounts[0], signature);
    } catch (e) {
      this.notificationService.notify('error', 'Failed to authenticate!');
      return false;
    }

    this.notificationService.notify('success', 'Succsessfuly authenticated!');
    return true;
  }

  public async getUserInfo(): Promise<UserInfo> {
    const accounts = await this.web3Service.getAccounts();
    if (accounts.length == 0) {
      return null;
    }
    const signature = this.getSignatureByAddress(accounts[0]);
    const userInfo = new UserInfo(accounts[0], signature, false);
    if (isNullOrUndefined(signature)) {
      return null;
    }
    const isSignatureValid = await this.web3Service.checkSignature(signature, accounts[0], AuthenticationService.MESSAGE_TO_SIGN);
    if (!isSignatureValid) {
      return null;
    }
    userInfo.isAuthenticated = true;
    return userInfo;
  }

  public getUserInfoAsObservable(): Observable<UserInfo> {
    return Observable.fromPromise(this.getUserInfo());
  }
}
