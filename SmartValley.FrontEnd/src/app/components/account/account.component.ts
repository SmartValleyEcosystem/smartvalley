import {Component, OnInit} from '@angular/core';
import {UserContext} from '../../services/authentication/user-context';
import {UserApiClient} from '../../api/user/user-api-client';
import {ErrorCode} from '../../shared/error-code.enum';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UpdateUserRequest} from '../../api/user/update-user-request';
import {User} from '../../services/authentication/user';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertUpdateRequest} from '../../api/expert/expert-update-request';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  public userForm: FormGroup;
  public currentUser: User;
  public about: string;

  constructor(private userContext: UserContext,
              private userApiClient: UserApiClient,
              private notificationsService: NotificationsService,
              private translateService: TranslateService,
              private expertApiClient: ExpertApiClient,
              private formBuilder: FormBuilder) {
    this.userContext.userContextChanged.subscribe(async (user) => {
      if (user) {
        this.currentUser = user;
        await this.updateInfoAsync();
      }
    });
  }

  async ngOnInit() {
    this.userForm = this.formBuilder.group({
      firstName: ['', [Validators.maxLength(50)]],
      secondName: ['', [Validators.maxLength(50)]],
      email: ['', [Validators.email, Validators.maxLength(50)]],
      about: ['', [Validators.maxLength(500)]]
    });

    this.currentUser = this.userContext.getCurrentUser();
    await this.updateInfoAsync();
  }

  public async saveAsync() {
    if (this.userForm.invalid) {
      this.notificationsService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Account.InvalidData')
      );
      return;
    }

    await Promise.all([this.updateEmailAsync(), this.updateUserDataAsync()]);

    await this.updateInfoAsync();
    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('Account.DataSaved')
    );
  }

  private async updateUserDataAsync(): Promise<void> {
    if (this.currentUser.isExpert) {
      await this.expertApiClient.updateAsync(<ExpertUpdateRequest>{
        firstName: this.userForm.value.firstName,
        secondName: this.userForm.value.secondName,
        about: this.userForm.value.about
      });
    } else {
      await this.userApiClient.updateAsync(<UpdateUserRequest>{
        firstName: this.userForm.value.firstName,
        secondName: this.userForm.value.secondName
      });
    }
  }

  private async updateEmailAsync(): Promise<void> {
    try {
      if (this.currentUser.email !== this.userForm.value.email) {
        await this.userApiClient.changeEmailAsync(this.userForm.value.email);
        this.notificationsService.info(
          this.translateService.instant('Account.EmailChanged'),
          this.translateService.instant('Account.ConfirmEmail')
        );
      }
    } catch (e) {
      if (e.error.errorCode === ErrorCode.EmailSendingFailed) {
        this.notificationsService.error(
          this.translateService.instant('Common.EmailSendingErrorTitle'),
          this.translateService.instant('Common.TryAgain')
        );
      } else if (e.error.errorCode === ErrorCode.EmailAlreadyExists) {
        this.notificationsService.error(
          this.translateService.instant('Common.EmailSendingErrorTitle'),
          this.translateService.instant('Common.EmailAlreadyExistErrorTitle')
        );
      }
    }
  }

  private async updateInfoAsync(): Promise<void> {
    const userResponse = await this.userApiClient.getByAddressAsync(this.currentUser.account);
    this.about = '';
    if (this.currentUser.isExpert) {
      const expertResponse = await this.expertApiClient.getAsync(this.currentUser.account);
      this.about = expertResponse.about;
    }
    this.userForm.setValue({
      firstName: userResponse.firstName,
      secondName: userResponse.secondName,
      email: this.currentUser.email,
      about: this.about
    });
  }
}
