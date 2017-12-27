import {Component, OnInit} from '@angular/core';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {BlockiesService} from '../../services/blockies-service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  public currentBalance: number;
  public currentTokens: number;
  public accountAddress: string;
  public accountImgUrl: string;

  constructor(private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalances(balance));
    this.authenticationService.accountChanged.subscribe((user) => {
      this.accountAddress = user ? user.account : '';
      this.accountImgUrl = user ? this.blockiesService.getImageForAddress(user.account) : '';
    });
  }

  async ngOnInit() {
    await this.balanceService.updateBalanceAsync();
    const currentUser = this.authenticationService.getCurrentUser();
    if (currentUser) {
      this.accountAddress = currentUser.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(currentUser.account);
    }
  }

  private updateBalances(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
    }
  }

}
