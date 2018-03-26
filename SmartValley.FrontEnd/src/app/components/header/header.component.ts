import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {UserContext} from '../../services/authentication/user-context';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public showReceiveEtherButton: boolean;
  public isAuthenticated: boolean;
  public isAdmin: boolean;
  public accountAddress: string;
  public accountImgUrl: string;
  public projectsLink: string;
  public accountLink: string;
  public adminPanelLink: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService,
              private userContext: UserContext) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.userContext.userContextChanged.subscribe((user) => this.updateAccount(user));
  }

  ngOnInit(): void {
    const currentUser = this.userContext.getCurrentUser();
    this.updateAccount(currentUser);
    this.projectsLink = Paths.Root;
    this.accountLink = Paths.Account;
    this.adminPanelLink = Paths.Admin;
  }

  private updateAccount(user: User): void {
    if (user) {
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.isAdmin = user.roles.includes('Admin');
    } else {
      this.isAuthenticated = false;
      this.isAdmin = false;
    }
  }

  public async loginAsync(): Promise<void> {
    await this.authenticationService.authenticateAsync();
  }

  public logout(): void {
    this.userContext.deleteCurrentUser();
  }

  private updateBalance(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.showReceiveEtherButton = !balance.wasEtherReceived;
    } else {
      this.showReceiveEtherButton = false;
    }
  }

  async receiveEth() {
    await this.balanceService.receiveEtherAsync();
  }

  async navigateToScoring() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.RegisterExpert]);
    }
  }

  async navigateToApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Application]);
    }
  }
}
