import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from '../../services/web3-service';
import {uptime} from 'os';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public wasEtherReceived: boolean;
  public isAuthenticated: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private web3Service: Web3Service) {
    this.authenticationService.accountChanged.subscribe(async () => await this.updateHeader());
  }

  async ngOnInit() {
    if (this.authenticationService.isAuthenticated()) {
      this.isAuthenticated = true;
      this.updateHeader();
    }
  }

  async updateHeader() {
    const response = await this.balanceApiClient.getBalanceAsync();
    this.currentBalance = response.balance;
    this.wasEtherReceived = response.wasEtherReceived;
  }


  async receiveEth() {
    await  this.balanceApiClient.receiveEtherAsync();
  }
}
