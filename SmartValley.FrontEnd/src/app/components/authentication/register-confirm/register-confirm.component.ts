import {Component, OnInit} from '@angular/core';
import {EmailRequest} from '../../../api/user/email-request';
import {UserApiClient} from '../../../api/user/user-api-client';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {Web3Service} from '../../../services/web3-service';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {AuthenticationApiClient} from '../../../api/authentication/authentication-api-client';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {isNullOrUndefined} from 'util';

@Component({
  selector: 'app-register-confirm',
  templateUrl: './register-confirm.component.html',
  styleUrls: ['./register-confirm.component.css']
})
export class RegisterConfirmComponent implements OnInit {

  private secondsKey = 'resendEmailCooldown';
  public email = '';
  public seconds: number;

  constructor(private userApiClient: UserApiClient,
              private web3Service: Web3Service,
              private router: Router,
              private notificationService: NotificationsService,
              private translateService: TranslateService,
              private authenticationApiClient: AuthenticationApiClient,
              private authenticationService: AuthenticationService) {
  }

  async ngOnInit() {
    const account = await this.web3Service.getCurrentAccountAsync();
    const signature = await this.authenticationService.getSignatureAsync(account);
    const response = await this.userApiClient.getEmailBySignatureAsync(<EmailRequest>{
      address: account,
      signature: signature,
      signedText: AuthenticationService.MESSAGE_TO_SIGN
    });
    this.email = response.email;

    const cooldown = localStorage.getItem(this.secondsKey);
    if (!isNullOrUndefined(cooldown)) {
      this.seconds = +cooldown;
    }
    setInterval(() => this.updateSeconds(), 1000);
  }

  navigateToRegister() {
    this.router.navigate([Paths.Register]);
  }

  public async sendEmailAsync() {
    const address = await this.web3Service.getCurrentAccountAsync();
    try {
      await this.authenticationApiClient.reSendEmailAsync(address);
      this.seconds = 60;
      localStorage.setItem(this.secondsKey, this.seconds.toString());
    } catch (e) {
      this.notificationService.error(
        this.translateService.instant('ConfirmEmailModal.ErrorTitle'),
        this.translateService.instant('ConfirmEmailModal.ErrorBody'));
    }
  }

  private updateSeconds() {
    if (this.seconds > 0) {
      this.seconds--;
      localStorage.setItem(this.secondsKey, this.seconds.toString());
    }
  }
}
