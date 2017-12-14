import {EventEmitter, Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {Web3Service} from '../web3-service';
import {NotificationsService} from 'angular2-notifications';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import 'rxjs/add/operator/map';
import {Ng2DeviceService} from 'ng2-device-detector';
import {MatDialog} from '@angular/material';
import {AlertModalComponent} from '../../components/common/alert-modal/alert-modal.component';
import {AlertModalData} from '../../components/common/alert-modal/alert-modal-data';
import {Constants} from '../../constants';
import {MetamaskManualModalData} from '../../components/common/metamask-manual-modal/metamask-manual-modal-data';
import {MetamaskManualModalComponent} from '../../components/common/metamask-manual-modal/metamask-manual-modal.component';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class AuthenticationService {
  constructor(private web3Service: Web3Service,
              private router: Router,
              private deviceService: Ng2DeviceService,
              private alertModal: MatDialog,
              private metamaskManualModel: MatDialog,
              private translateService: TranslateService) {
    if (this.web3Service.isMetamaskInstalled && this.getCurrentUser() != null) {
      this.startBackgroundChecker();
    }
  }

  public static MESSAGE_TO_SIGN = 'Confirm login. Please press the \'Sign\' button below.';
  public accountChanged: EventEmitter<any> = new EventEmitter<any>();

  private readonly userKey = 'userKey';
  private backgroundChecker: Subscription;
  private readonly compatibleBrowsers = [Constants.Chrome, Constants.Firefox];

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
      this.showIncompatibleBrowserAlert();
      return;
    }

    if (!this.web3Service.isMetamaskInstalled) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return;
    }
    const accounts = await this.web3Service.getAccounts();
    const currentAccount = accounts[0];

    if (currentAccount == null) {
      this.showUnlockAccountAlert();
      return;
    }

    const isRinkeby = await this.web3Service.checkRinkebyNetworkAsync();

    if (!isRinkeby) {
      this.showRinkebyAlert();
      return;
    }

    let signature = this.getSignatureByAccount(currentAccount);
    const shouldSign = await this.shouldSignAccount(currentAccount, signature);
    if (shouldSign) {
      try {
        signature = await this.web3Service.sign(AuthenticationService.MESSAGE_TO_SIGN, currentAccount);
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
    const isValid = await this.checkSignatureAsync(user.account, user.signature);
    return !isValid;
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

  public stopUserSession() {
    this.deleteCurrentUser();
    this.stopBackgroundChecker();
    this.router.navigate([Paths.Root]);
  }

  private showIncompatibleBrowserAlert() {
    this.alertModal.open(AlertModalComponent, {
      width: '600px',
      data: <AlertModalData>{
        title: this.translateService.instant('Authentication.IncompatibleBrowserTitle'),
        message: this.translateService.instant('Authentication.IncompatibleBrowserMessage'),
        button: this.translateService.instant('Authentication.IncompatibleBrowserButton')
      }
    });
  }

  private showUnlockAccountAlert() {
    this.metamaskManualModel.open(MetamaskManualModalComponent, {
      width: '500px',
      data: <MetamaskManualModalData>{
        title: this.translateService.instant('Authentication.UnlockMetamaskTitle'),
        message: this.translateService.instant('Authentication.UnlockMetamaskMessage'),
        button: this.translateService.instant('Authentication.UnlockMetamaskButton'),
        imgUrl: '/assets/img/unlock_metamask.png'
      }
    });
  }

  private showRinkebyAlert() {
    this.metamaskManualModel.open(MetamaskManualModalComponent, {
      width: '500px',
      data: <MetamaskManualModalData>{
        title: this.translateService.instant('Authentication.WrongNetworkTitle'),
        message: this.translateService.instant('Authentication.WrongNetworkMessage'),
        button: this.translateService.instant('Authentication.WrongNetworkButton'),
        imgUrl: '/assets/img/change_network.png'
      }
    });
  }
}
