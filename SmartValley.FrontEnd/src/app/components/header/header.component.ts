import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {TokenReceivingService} from '../../services/token-receiving/token-receiving-service';
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
  public showReceiveSVTButton: boolean;
  public showBalance: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private router: Router,
              private tokenService: TokenReceivingService,
              private etherReceivingService: EtherReceivingService) {
    this.authenticationService.accountChanged.subscribe(async () => await this.updateHeaderAsync());
  }

  async ngOnInit() {
    await this.updateHeaderAsync();
  }

  async updateHeaderAsync(): Promise<void> {
    if (this.authenticationService.isAuthenticated()) {
      this.showBalance = true;
      const balanceResponse = await this.balanceApiClient.getBalanceAsync();
      this.currentBalance = +balanceResponse.balance.toFixed(3);
      const address = this.authenticationService.getCurrentUser().account;
      this.currentTokens = await this.tokenService.getBalanceAsync(address);
      this.showReceiveEtherButton = !balanceResponse.wasEtherReceived;
      this.showReceiveSVTButton = await this.tokenService.canGetTokensAsync(address);
    } else {
      this.showBalance = false;
      this.showReceiveEtherButton = false;
      this.showReceiveSVTButton = false;
    }
  }

  async receiveEth() {
    await this.etherReceivingService.receiveAsync();
    await this.updateHeaderAsync();
  }

  async receiveSVT() {
    await this.tokenService.receiveAsync();
    await this.updateHeaderAsync();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
