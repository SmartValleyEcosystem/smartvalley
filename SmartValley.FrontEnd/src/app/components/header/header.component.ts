import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {UserContext} from '../../services/authentication/user-context';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';

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
              private userContext: UserContext,
              private expertApiClient: ExpertApiClient) {
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.userContext.userContextChanged.subscribe((user) => this.updateAccount(user));
  }

  ngOnInit(): void {
    const currentUser = this.userContext.getCurrentUser();
    this.updateAccount(currentUser);
    this.projectsLink = Paths.ProjectList;
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

  async navigateToExpertApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      const address = this.userContext.getCurrentUser().account;
      const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
      if (expertStatusResponse.status === ExpertApplicationStatus.Pending) {
        await this.router.navigate([Paths.ExpertStatus]);
      } else if (expertStatusResponse.status === ExpertApplicationStatus.Accepted) {
        await this.router.navigate([Paths.Expert]);
      } else {
        await this.router.navigate([Paths.RegisterExpert]);
      }
    }
  }

  async navigateToProjectApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Project]);
    }
  }
}
