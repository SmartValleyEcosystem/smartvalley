import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {TokenReceivingService} from '../../services/token-receiving/token-receiving-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {EtherReceivingService} from '../../services/ether-receiving/ether-receiving-service';
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

  constructor(private balanceApiClient: BalanceApiClient,
              private router: Router,
              private balanceService: BalanceService,
              private authenticationService: AuthenticationService,
              private etherReceivingService: EtherReceivingService,
              private tokenReceivingService: TokenReceivingService) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateHeader(balance));
    this.updateHeader(this.balanceService.balance);
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
    await this.etherReceivingService.receiveAsync();

  }

  async receiveSVT() {
    await this.tokenReceivingService.receiveAsync();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
