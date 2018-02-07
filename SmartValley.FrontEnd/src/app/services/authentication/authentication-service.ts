import {EventEmitter, Injectable} from '@angular/core';
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
import {NotificationsService} from 'angular2-notifications';
import {AdminContractClient} from '../contract-clients/admin-contract-client';
import {AccountSignatureDto} from './account-signature-dto';
import {RegistrationRequest} from '../../api/authentication/registration-request';
import {AuthenticationRequest} from '../../api/authentication/authentication-request';


@Injectable()
export class AuthenticationService {
  public static MESSAGE_TO_SIGN = 'Confirm login. Please press the \'Sign\' button below.';

  private backgroundChecker: Subscription;
  private readonly compatibleBrowsers = [Constants.Chrome, Constants.Firefox];

  constructor(private web3Service: Web3Service,
              private router: Router,
              private deviceService: Ng2DeviceService,
              private dialogService: DialogService,
              private userContext: UserContext,
              private authenticationApiClient: AuthenticationApiClient,
              private notificationsService: NotificationsService,
              private adminContractClient: AdminContractClient) {

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

  public isAuthenticated() {
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

    const currentAccount = await this.web3Service.getCurrentAccountAsync();
    if (currentAccount == null) {
      this.dialogService.showUnlockAccountAlert();
      return false;
    }

    if (!await this.web3Service.checkNetworkAsync()) {
      this.dialogService.showRinkebyAlert();
      return false;
    }

    let signature = this.userContext.getSignatureByAccount(currentAccount);
    if (await this.shouldSignAccountAsync(currentAccount, signature)) {
      try {
        signature = await this.web3Service.signAsync(AuthenticationService.MESSAGE_TO_SIGN, currentAccount);
      } catch (e) {
        return false;
      }
    }

    const user = await this.authenticateOnBackendAsync(currentAccount, signature, AuthenticationService.MESSAGE_TO_SIGN);
    if (user == null) {
      return false;
    }

    this.startUserSession(user);
    return true;
  }

  private async authenticateOnBackendAsync(account: string, signature: string, messageToSign: string): Promise<User> {
    const user = this.userContext.getCurrentUser();
    if (user != null && user.account === account) {
      return user;
    }
    try {
      const response = await this.authenticationApiClient.authenticateAsync(<AuthenticationRequest>{
        address: account,
        signature: signature,
        signedText: messageToSign
      });

      const isAdmin = await this.adminContractClient.isAdminAsync(account);
      return <User>{
        account: account,
        signature: signature,
        token: response.token,
        roles: response.roles,
        isAdmin: isAdmin
      };
    } catch (e) {
      if (e.error.errorCode === ErrorCode.UserNotFound) {
        const isSuccess = await this.registerAsync(account, signature, messageToSign);

        if (isSuccess) {
          this.notificationsService.success('Successful registration', 'Please check your email');
        } else {
          this.notificationsService.error('Failed registration', 'Please try again later');
        }
        return null;
      }
    }
  }

  private async registerAsync(account: string, signature: string, messageToSign: string): Promise<boolean> {
    try {
      const email = await this.dialogService.showRegisterDialogAsync();
      if (isNullOrUndefined(email)) {
        return false;
      }
      await this.authenticationApiClient.registerAsync(<RegistrationRequest>{
        address: account,
        email: email,
        signedText: messageToSign,
        signature: signature
      });
      return true;
    } catch (e) {
      return false;
    }
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
    const isSavedSignatureValid = await this.checkSignatureAsync(currentAccount, savedSignature);
    if (isSavedSignatureValid) {
      user = await this.authenticateOnBackendAsync(currentAccount, savedSignature, AuthenticationService.MESSAGE_TO_SIGN);
      this.userContext.saveCurrentUser(user);
    } else {
      this.userContext.deleteCurrentUser();
    }
  }
}
