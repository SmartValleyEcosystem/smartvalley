import {Component, OnInit} from '@angular/core';
import {UserContext} from '../../services/authentication/user-context';
import {UserApiClient} from '../../api/user/user-api-client';
import {ErrorCode} from '../../shared/error-code.enum';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UpdateUserRequest} from '../../api/user/update-user-request';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';
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
  public isExpert: boolean;

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

  public async updateEmailAsync(): Promise<void> {
    try {
      if (this.currentUser.email !== this.userForm.value.email) {
        await this.userApiClient.changeEmailAsync(this.userForm.value.email);
      }
    } catch (e) {
      if (e.error.errorCode === ErrorCode.EmailSendingFailed) {
        this.notificationsService.error(
          this.translateService.instant('Common.EmailSendingErrorTitle'),
          this.translateService.instant('Common.TryAgain')
        );
      }
    }
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
    if (this.isExpert) {
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

  private async updateInfoAsync(): Promise<void> {
    const userResponse = await this.userApiClient.getByAddressAsync(this.currentUser.account);
    this.about = '';
    this.isExpert = false;
    const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(this.currentUser.account);
    if (expertStatusResponse.status === ExpertApplicationStatus.Accepted) {
      const expertResponse = await this.expertApiClient.getAsync(this.currentUser.account);
      this.about = expertResponse.about;
      this.isExpert = true;
    }
    this.userForm.setValue({
      firstName: userResponse.firstName,
      secondName: userResponse.secondName,
      email: this.currentUser.email,
      about: this.about
    });
  }
}
