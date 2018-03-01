import {Component, OnInit} from '@angular/core';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {Router} from '@angular/router';
import {UserContext} from '../../services/authentication/user-context';
import {UserApiClient} from '../../api/user/user-api-client';
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
  public userForm: FormGroup;
  private currentUser: User;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private userContext: UserContext,
              private userApiClient: UserApiClient,
              private formBuilder: FormBuilder) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalances(balance));
    this.userContext.userContextChanged.subscribe((user) => this.updateAccount(user));
  }

  private updateAccount(user: User): void {
    if (user) {
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
    }
  }

  async ngOnInit() {
    this.userForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      about: ['', [Validators.maxLength(1500)]]
    });

    await this.balanceService.updateBalanceAsync();
    this.currentUser = this.userContext.getCurrentUser();

    this.updateAccount(this.currentUser);
    this.updateInfo();
  }

  public async saveChangesAsync() {
    await this.userApiClient.updateAsync(<UpdateUserRequest>{
      address: this.currentUser.account,
      about: this.userForm.value.about,
      name: this.userForm.value.name
    });

    this.updateInfo();
  }

  private updateBalances(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.transferableTokens = balance.availableBalance;
      this.frozenTokens = +(balance.svtBalance - balance.availableBalance).toFixed(3);
    }
  }

  private async updateInfo() {
    const userResponse = await this.userApiClient.getByAddressAsync(this.currentUser.account);
    this.userForm.setValue({
      name: userResponse.name,
      about: userResponse.about
    });
  }
}
