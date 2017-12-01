import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Web3Service} from '../../services/web3-service';
import {MatDialog} from '@angular/material';
import {NotificationsService} from 'angular2-notifications';
import {Constants} from '../../constants';
import {DialogService} from '../../services/dialog-service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public showReceiveEtherButton: boolean;
  public showBalance: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private router: Router,
              private web3Service: Web3Service,
              private projectModal: MatDialog,
              private notificationsService: NotificationsService,
              private dialogService: DialogService) {
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
      this.showReceiveEtherButton = !balanceResponse.wasEtherReceived;
    } else {
      this.showBalance = false;
      this.showReceiveEtherButton = false;
    }
  }

  async receiveEth() {
    await this.dialogService.showReceiveEthDialog();
    await this.updateHeaderAsync();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
