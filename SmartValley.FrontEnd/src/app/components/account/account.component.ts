import { Component, OnInit } from '@angular/core';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {AuthenticationService} from '../../services/authentication/authentication-service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  public currentBalance: number;
  public currentTokens: number;
  public accountAddress: string;

  constructor(
    private balanceService: BalanceService,
    private authenticationService: AuthenticationService) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalances(balance));
  }

  async ngOnInit() {
    await this.balanceService.updateBalanceAsync();
    const currentUser = this.authenticationService.getCurrentUser();
    if (currentUser) {
      this.accountAddress = currentUser.account;
    }
  }

  private updateBalances(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
    }
  }

}
