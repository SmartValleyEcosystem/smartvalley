import {Component, OnInit} from '@angular/core';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Web3Service} from '../../services/web3-service';
import {TransactionAwaitingModalComponent} from '../common/transaction-awaiting-modal/transaction-awaiting-modal.component';
import {MatDialog, MatDialogRef} from '@angular/material';
import {TransactionAwaitingModalData} from '../common/transaction-awaiting-modal/transaction-awaiting-modal-data';
import {NotificationsService} from 'angular2-notifications';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public showReceiveEtherButton: boolean;
  public showBalance: boolean;
  private projectModalRef: MatDialogRef<TransactionAwaitingModalComponent>;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private router: Router,
              private web3Service: Web3Service,
              private projectModal: MatDialog,
              private notificationsService: NotificationsService) {
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
    const transactionHash = (await this.balanceApiClient.receiveEtherAsync()).transactionHash;

    this.openProjectModal(
      'Ether transfer is in progress. Please wait for completion of transaction.',
      transactionHash
    );

    try {
      await this.web3Service.waitForConfirmationAsync(transactionHash);

      this.notificationsService.success('Ether transfer completed.');
    } catch (e) {
      this.notificationsService.error('Ether transfer failed. Please try again later.');
    }

    this.closeProjectModal();
    await this.updateHeaderAsync();
  }

  private openProjectModal(message: string, transactionHash: string) {
    this.projectModalRef = this.projectModal.open(TransactionAwaitingModalComponent, {
      width: '600px',
      data: <TransactionAwaitingModalData>{
        message: message,
        transactionHash: transactionHash
      },
      disableClose: true,
    });
  }

  private closeProjectModal() {
    this.projectModal.closeAll();
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: 'myProjects'}});
    }
  }
}
