import {Component, OnInit} from '@angular/core';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {Router} from '@angular/router';
import {UserContext} from '../../services/authentication/user-context';

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

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private userContext: UserContext) {

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
    await this.balanceService.updateBalanceAsync();
    const currentUser = this.userContext.getCurrentUser();
    this.updateAccount(currentUser);
  }

  private updateBalances(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.transferableTokens = balance.availableBalance;
      this.frozenTokens = +(balance.svtBalance - balance.availableBalance).toFixed(3);
    }
  }
}
