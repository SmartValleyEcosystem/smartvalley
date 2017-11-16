import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';
import {NotificationService} from './notification-service';
import {UserInfo} from './user-info';

@Injectable()
export class AuthenticationService {

  static MESSAGE_TO_SIGN = 'Confirm login';

  public userInfoChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service, private _notificationService: NotificationService) {
  }

  public getSignatureByAddress(account: string) {
    return window.localStorage[account];
  }

  public saveSignatureForAddrsess(account: string, signature: string) {
    window.localStorage[account] = signature;
  }

  public removeSignatureByAddress(account: string) {
    window.localStorage.removeItem(account);
  }

  public async authenticate(): Promise<Boolean> {
    try {
      const accounts = await this.web3Service.getAccounts();
      const signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, accounts[0]);
      this.saveSignatureForAddrsess(accounts[0], signature);
    } catch (e) {
      this._notificationService.notify('error', 'Failed to authenticate!');
      return false;
    }

    this._notificationService.notify('success', 'Successfully authenticated!');
    this.userInfoChanged.emit();
    return true;
  }

  public async getUserInfo(): Promise<UserInfo> {
    const accounts = await this.web3Service.getAccounts();
    if (accounts.length == 0) {
      return null;
    }
    const signature = this.getSignatureByAddress(accounts[0]);

    if (isNullOrUndefined(signature)) {
      return null;
    }
    const isSignatureValid = await this.web3Service.checkSignature(signature, accounts[0], AuthenticationService.MESSAGE_TO_SIGN);
    if (!isSignatureValid) {
      return null;
    }
    const userInfo = new UserInfo(accounts[0], signature, false);
    userInfo.isAuthenticated = true;
    return userInfo;
  }
}
