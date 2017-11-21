import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from './web3-service';
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

  public static MESSAGE_TO_SIGN = 'Confirm login';

  private readonly userKey = 'userKey';

  private getSignatureByAccount(account: string) {
    return localStorage.getItem(account);
  }

  private saveSignatureForAccount(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  private removeSignatureByAccount(account: string) {
    localStorage.removeItem(account);
  }

  public async authenticateAsync(): Promise<Boolean> {

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
    const isRinkeby = await this.web3Service.checkRinkebyNetworkAsync();

    if (!isRinkeby) {
      this.notificationsService.warn('Wrong network', 'Please change to Rinkeby');
      return;
    }

    const user = this.getCurrentUser();
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
    const savedSignature = this.getSignatureByAccount(account);
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
    const signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, account);
    this.saveCurrentUser({account, signature});
    this.saveSignatureForAccount(account, signature);
  }

  private async checkSignatureAsync(account: string, signature: string): Promise<Boolean> {
    const recoveredSignature = await this.web3Service.recoverSignature(AuthenticationService.MESSAGE_TO_SIGN, signature);
    return account === recoveredSignature;
  }

  public getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(this.userKey));
  }

  private saveCurrentUser(user: User) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }
}
