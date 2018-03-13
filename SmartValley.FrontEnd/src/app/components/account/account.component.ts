import {Component, OnInit} from '@angular/core';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {Router} from '@angular/router';
import {UserContext} from '../../services/authentication/user-context';
import {UserApiClient} from '../../api/user/user-api-client';
import {DialogService} from '../../services/dialog-service';
import {isNullOrUndefined} from 'util';
import {ErrorCode} from '../../shared/error-code.enum';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UpdateUserRequest} from '../../api/user/update-user-request';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  public currentBalance: number;
  public currentTokens: number;
  public transferableTokens: number;
  public frozenTokens: number;
  public accountAddress: string;
  public accountImgUrl: string;
  public accountEmail: string;
  public userForm: FormGroup;
  private currentUser: User;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private userContext: UserContext,
              private userApiClient: UserApiClient,
              private dialogService: DialogService,
              private notificationsService: NotificationsService,
              private translateService: TranslateService,
              private formBuilder: FormBuilder) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalances(balance));
    this.userContext.userContextChanged.subscribe((user) => this.updateAccountAsync(user));
  }

  public async changeEmailAsync(): Promise<void> {
    const address = this.userContext.getCurrentUser().account;
    const newEmail = await this.dialogService.showChangeEmailDialogAsync();

    try {
      if (!isNullOrUndefined(newEmail)) {
        await this.userApiClient.changeEmailAsync(address, newEmail);
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

  private async updateAccountAsync(user: User): Promise<void> {
    if (user) {
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.accountEmail = user.email;
    }
  }

  async ngOnInit() {
    this.userForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      about: ['', [Validators.maxLength(1500)]]
    });

    await this.balanceService.updateBalanceAsync();
    this.currentUser = this.userContext.getCurrentUser();

    await this.updateAccountAsync(this.currentUser);
    await this.updateInfoAsync();
  }

  public async saveChangesAsync() {
    if (this.userForm.invalid) {
      this.notificationsService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Account.InvalidData')
      );
      return;
    }
    await this.userApiClient.updateAsync(<UpdateUserRequest>{
      address: this.currentUser.account,
      about: this.userForm.value.about,
      name: this.userForm.value.name
    });

    await this.updateInfoAsync();
    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('Account.DataSaved')
    );
  }

  private updateBalances(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.transferableTokens = balance.availableBalance;
      this.frozenTokens = +(balance.svtBalance - balance.availableBalance).toFixed(3);
    }
  }

  private async updateInfoAsync(): Promise<void> {
    const userResponse = await this.userApiClient.getByAddressAsync(this.currentUser.account);
    this.userForm.setValue({
      name: userResponse.name,
      about: userResponse.about
    });
  }
}
