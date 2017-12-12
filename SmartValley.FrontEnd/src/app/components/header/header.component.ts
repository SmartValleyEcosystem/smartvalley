import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {TokenClient} from '../../api/token/token-client';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {EtherReceivingService} from '../../services/ether-receiving/ether-receiving-service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public currentTokens: number;
  public showReceiveEtherButton: boolean;
  public showBalance: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private router: Router,
              private tokenClient: TokenClient,
              private etherReceivingService: EtherReceivingService) {
    this.authenticationService.accountChanged.subscribe(async () => await this.updateHeaderAsync());
  }

  async ngOnInit() {
    this.updateHeaderAsync();
  }

  async updateHeaderAsync(): Promise<void> {
    if (this.authenticationService.isAuthenticated()) {
      this.showBalance = true;
      const balanceResponse = await this.balanceApiClient.getBalanceAsync();
      this.currentBalance = +balanceResponse.balance.toFixed(3);
      const address = this.authenticationService.getCurrentUser().account;
      this.currentTokens = await this.tokenClient.getTokenBalanceFromAddress(address);
      this.showReceiveEtherButton = !balanceResponse.wasEtherReceived;
    } else {
      this.showBalance = false;
      this.showReceiveEtherButton = false;
    }
  }

  async receiveEth() {
    await this.etherReceivingService.receiveAsync();
    await this.updateHeaderAsync();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
