import {Component, OnInit} from "@angular/core";
import {BalanceApiClient} from "../../api/balance/balance-api-client";
import {AuthenticationService} from "../../services/authentication-service";
import {Web3Service} from "../../services/web3-service";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.css"]
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public wasEtherReceived: boolean;
  public isAuthenticated: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private web3Service: Web3Service) {

  }

  async ngOnInit() {
  }

  async updateHeader() {

  }


  async receiveEth() {
    await  this.balanceApiClient.receiveEther();
  }
}
