import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  public currentBalance: number;
  public currentTokens: number;
  public showReceiveEtherButton: boolean;
  public showReceiveSVTButton: boolean;
  public showBalance: boolean;
  public accountAddress: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private authenticationService: AuthenticationService) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateHeader(balance));
    this.authenticationService.accountChanged.subscribe((user) => this.accountAddress = user ? user.account : '');
    const currentUser = this.authenticationService.getCurrentUser();
    if (currentUser) {
      this.accountAddress = currentUser.account;
    }
    this.updateHeader(this.balanceService.balance);
  }

  public async loginAsync(): void {
    const isOk = await this.authenticationService.authenticateAsync();
  }

  public logout(): void{
    this.authenticationService.stopUserSession();
  }

  private updateHeader(balance: Balance): void {
    if (balance != null) {
      this.showBalance = true;
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.showReceiveEtherButton = !balance.wasEtherReceived;
      this.showReceiveSVTButton = balance.canReceiveSvt;
    } else {
      this.showBalance = false;
      this.showReceiveEtherButton = false;
      this.showReceiveSVTButton = false;
    }
  }

  async receiveEth() {
    await this.balanceService.receiveSvtAsync();
  }

  async receiveSVT() {
    await this.balanceService.receiveEtherAsync();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
