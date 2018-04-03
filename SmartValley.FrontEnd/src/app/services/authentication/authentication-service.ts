import {Injectable} from '@angular/core';
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
import {UserContext} from './user-context';
import {ErrorCode} from '../../shared/error-code.enum';
import {AuthenticationApiClient} from '../../api/authentication/authentication-api-client';
import {AccountSignatureDto} from './account-signature-dto';
import {AuthenticationRequest} from '../../api/authentication/authentication-request';
import {RegistrationRequest} from '../../api/authentication/registration-request';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {UserApiClient} from '../../api/user/user-api-client';

@Injectable()
export class AuthenticationService {
  public static MESSAGE_TO_SIGN = 'Confirm login. Please press the \'Sign\' button below.';

  private backgroundChecker: Subscription;
  private readonly compatibleBrowsers = [Constants.Chrome, Constants.Firefox];

  constructor(private web3Service: Web3Service,
              private router: Router,
              private deviceService: Ng2DeviceService,
              private dialogService: DialogService,
              private translateService: TranslateService,
              private notificationsService: NotificationsService,
              private userContext: UserContext,
              private authenticationApiClient: AuthenticationApiClient,
              private userApiClient: UserApiClient) {

    this.userContext.userContextChanged.subscribe((user) => {
      if (user == null) {
        this.stopBackgroundChecker();
        this.router.navigate([Paths.Root]);
      }
    });
  }

  public async initializeAsync(): Promise<void> {
    if (this.web3Service.isMetamaskInstalled && this.userContext.getCurrentUser() != null) {
      this.startBackgroundChecker();
    }
  }

  public isAuthenticated(): boolean {
    return this.web3Service.isMetamaskInstalled && !isNullOrUndefined(this.userContext.getCurrentUser());
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

    const currentAccountAddress = await this.web3Service.getCurrentAccountAsync();
    if (currentAccountAddress == null) {
      this.dialogService.showUnlockAccountAlert();
      return false;
    }

    if (!await this.web3Service.checkNetworkAsync()) {
      this.dialogService.showMetamaskManualAlert();
      return false;
    }

    if (!await this.isRegistrationCompletedAsync(currentAccountAddress)) {
      return false;
    }

    let signature: string;
    try {
      signature = await this.getSignatureAsync(currentAccountAddress);
    } catch (e) {
      return false;
    }
    const user = await this.authenticateOnBackendAsync(currentAccountAddress, signature, AuthenticationService.MESSAGE_TO_SIGN);
    if (user == null) {
      return false;
    }
    this.startUserSession(user);
    return true;
  }

  private async isRegistrationCompletedAsync(address: string): Promise<boolean> {
    const userResponse = await this.userApiClient.getByAddressAsync(address);
    if (isNullOrUndefined(userResponse.address)) {
      await this.router.navigate([Paths.Register]);
      return false;
    }

    if (!userResponse.isEmailConfirmed) {
      await this.router.navigate([Paths.ConfirmRegister]);
      return false;
    }

    return true;
  }

  private async authenticateOnBackendAsync(account: string, signature: string, messageToSign: string): Promise<User> {
    const user = this.userContext.getCurrentUser();
    if (user != null && user.account === account) {
      return user;
    }

    const response = await this.authenticationApiClient.authenticateAsync(<AuthenticationRequest>{
      address: account,
      signature: signature,
      signedText: messageToSign
    });

    return <User>{
      email: response.email,
      account: account,
      signature: signature,
      token: response.token,
      roles: response.roles
    };
  }

  public async getSignatureAsync(currentAccount: string): Promise<string> {
    const signature = this.userContext.getSignatureByAccount(currentAccount);
    if (await this.shouldSignAccountAsync(currentAccount, signature)) {
      return await this.web3Service.signAsync(AuthenticationService.MESSAGE_TO_SIGN, currentAccount);
    }
    return signature;
  }

  private async shouldSignAccountAsync(currentAccount: string, savedSignature: string): Promise<boolean> {
    const accountSignature = this.getAccountAndSignatureToCheck(currentAccount, savedSignature);
    return !await this.checkSignatureAsync(accountSignature.account, accountSignature.signature);
  }

  private getAccountAndSignatureToCheck(currentAccount: string, savedSignature: string): AccountSignatureDto {
    const user = this.userContext.getCurrentUser();
    if (user == null || user.account !== currentAccount) {
      return <AccountSignatureDto>{
        account: currentAccount,
        signature: savedSignature
      };
    }
    return <AccountSignatureDto>{
      account: user.account,
      signature: user.signature
    };
  }

  private startUserSession(newUser: User) {
    const user = this.userContext.getCurrentUser();
    if (user != null && user.account === newUser.account && user.signature === newUser.signature) {
      return;
    }
    this.userContext.saveCurrentUser(newUser);
    this.userContext.saveSignatureForAccount(newUser.account, newUser.signature);
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

  public async registerAsync(email: string): Promise<void> {
    const account = await this.web3Service.getCurrentAccountAsync();
    const signature = await this.getSignatureAsync(account);

    try {
      await this.authenticationApiClient.registerAsync(<RegistrationRequest>{
        address: account,
        email: email,
        signedText: AuthenticationService.MESSAGE_TO_SIGN,
        signature: signature
      });
      await this.userContext.saveSignatureForAccount(account, signature);
      await this.router.navigate([Paths.ConfirmRegister]);
    } catch (e) {
      switch (e.error.errorCode) {
        case ErrorCode.EmailSendingFailed:
          this.notificationsService.error(
            this.translateService.instant('Common.EmailSendingErrorTitle'),
            this.translateService.instant('Common.TryAgain')
          );
          break;
        case ErrorCode.EmailAlreadyExists:
          this.notificationsService.error(
            this.translateService.instant('Common.EmailAlreadyExistErrorTitle'),
            this.translateService.instant('Common.EnterAnotherEmail')
          );
          break;
      }
    }
  }

  private async checkCurrentAuthStateAsync(): Promise<void> {
    const currentAccount = await this.web3Service.getCurrentAccountAsync();
    let user = this.userContext.getCurrentUser();
    if (isNullOrUndefined(user)) {
      this.userContext.deleteCurrentUser();
    }
    if (user.account === currentAccount) {
      return;
    }
    const savedSignature = this.userContext.getSignatureByAccount(currentAccount);
    if (await this.checkSignatureAsync(currentAccount, savedSignature)) {
      user = await this.authenticateOnBackendAsync(currentAccount, savedSignature, AuthenticationService.MESSAGE_TO_SIGN);
      this.userContext.saveCurrentUser(user);
    } else {
      this.userContext.deleteCurrentUser();
    }
  }
}
