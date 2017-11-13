import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from "../../api/balance/balance-api-client";
import {LoginInfoService} from "../../services/login-info-service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: string;
  public hadReceiviedEther: boolean;

  constructor(private balanceApiClient: BalanceApiClient, private loginInfoService: LoginInfoService) {

  }


  async ngOnInit() {
    const isLoggedIn = this.loginInfoService.isAuthenticated();
    if (isLoggedIn) {
      const result = await  this.balanceApiClient.getBalance();
      this.currentBalance = result.balance.toString();
      this.hadReceiviedEther = result.HadReceiviedEther;
    }
  }

  async receieveEth() {
    await this.balanceApiClient.receiveEther();
  }
}
