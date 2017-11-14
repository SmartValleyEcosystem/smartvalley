import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from "../../services/web3-service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public wasEtherReceived: boolean;
  public isAunthenticated: boolean;

  constructor(private balanceApiClient: BalanceApiClient, private authenticationService: AuthenticationService, private web3Service: Web3Service) {
    this.authenticationService.userInfoChanged.subscribe(async () => await this.updateHeader());
  }

  async ngOnInit() {
    if (!this.web3Service.isInitialized) {
      return;
    }
    await this.updateHeader();
  }

  async updateHeader() {
    const userInfo = await this.authenticationService.getUserInfo();
    if (userInfo != null && userInfo.isAuthenticated) {
      const result = await this.balanceApiClient.getBalance();
      this.currentBalance = result.balance;
      this.wasEtherReceived = result.wasEtherReceived;
      this.isAunthenticated = true;
    }
  }

  async receieveEth() {
    await this.balanceApiClient.receiveEther();
  }
}
