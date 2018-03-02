import {Component, Inject, OnInit} from '@angular/core';
import {AuthenticationApiClient} from '../../../api/authentication/authentication-api-client';
import {Web3Service} from '../../../services/web3-service';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {isNullOrUndefined} from 'util';
import {MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-confirm-email-modal',
  templateUrl: './confirm-email-modal.component.html'
})
export class ConfirmEmailModalComponent implements OnInit {

  private secondsKey = 'resendEmailCooldown';

  constructor(@Inject(MAT_DIALOG_DATA) public email: string,
              private authenticationApiClient: AuthenticationApiClient,
              private web3Service: Web3Service,
              private notificationService: NotificationsService,
              private translateService: TranslateService) {
  }

  public seconds: number;

  ngOnInit(): void {
    this.seconds = 0;
    const cooldown = localStorage.getItem(this.secondsKey);
    if (!isNullOrUndefined(cooldown)) {
      this.seconds = +cooldown;
    }
    setInterval(() => this.updateSeconds(), 1000);
  }

  public async sendEmail() {
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
