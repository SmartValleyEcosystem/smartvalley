import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';
import {NotificationService} from './notification-service';

@Injectable()
export class AuthenticationService {


  // public userInfoChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private web3Service: Web3Service, private _notificationService: NotificationService) {
  }

  public static MESSAGE_TO_SIGN = 'Confirm login';

  private readonly userKey = 'userKey';

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
    let user = this.getCurrentUser();
    if (!isNullOrUndefined(user)) {
      const recoveredSignature = await this.web3Service.recoverSignature(AuthenticationService.MESSAGE_TO_SIGN, user.signature);
      if (user.account === recoveredSignature) {
        return true;
      }
    }

    const accounts = await this.web3Service.getAccounts();
    const account = accounts[0];
    const signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, account);

    user = {account, signature};
    this.saveCurrent(user);
    this.saveSignatureForAddrsess(account, signature);
    return true;
  }

  public getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveCurrent(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }
}
