import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {AdminApiClient} from '../../api/admin/admin-api-client';
import {AdminContractClient} from '../../services/contract-clients/admin-contract-client';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public currentTokens: number;
  public frozenTokens: number;
  public showReceiveEtherButton: boolean;
  public showReceiveSVTButton: boolean;
  public isAuthenticated: boolean;
  public isAdmin: boolean;
  public accountAddress: string;
  public accountImgUrl: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.authenticationService.accountChanged.subscribe(async (user) => await this.updateAccount(user));
    this.updateBalance(this.balanceService.balance);
  }

  public async ngOnInit(): Promise<void> {
    const currentUser = this.authenticationService.getCurrentUser();
    await this.updateAccount(currentUser);
  }

  private async updateAccount(user: User): Promise<void> {
    if (user) {
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.isAdmin = user.isAdmin;
    } else {
      this.isAuthenticated = false;
      this.isAdmin = false;
    }
  }

  public async loginAsync(): Promise<void> {
    await this.authenticationService.authenticateAsync();
  }

  public logout(): void {
    this.authenticationService.stopUserSession();
  }

  private updateBalance(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.frozenTokens = +(balance.svtBalance - balance.availableBalance).toFixed(3);
      this.showReceiveEtherButton = !balance.wasEtherReceived;
      this.showReceiveSVTButton = balance.canReceiveSvt;
    } else {
      this.showReceiveEtherButton = false;
      this.showReceiveSVTButton = false;
    }
  }

  async receiveEth() {
    await this.balanceService.receiveEtherAsync();
  }

  async receiveSVT() {
    await this.balanceService.receiveSvtAsync();
  }

  async navigateToAdminPanel() {
    await this.router.navigate([Paths.Admin]);
  }

  async navigateToAccount() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Account]);
    }
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.MyProjects]);
    }
  }
}
