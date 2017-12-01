import {Component} from '@angular/core';
import {MatDialog, MatDialogRef} from '@angular/material';
import {Router} from '@angular/router';
import {Web3Service} from '../../../services/web3-service';
import {NotificationsService} from 'angular2-notifications';
import {AuthenticationService} from '../../../services/authentication-service';
import {BalanceApiClient} from '../../../api/balance/balance-api-client';
import {TransactionAwaitingModalComponent} from '../transaction-awaiting-modal/transaction-awaiting-modal.component';
import {TransactionAwaitingModalData} from '../transaction-awaiting-modal/transaction-awaiting-modal-data';

@Component({
  selector: 'app-get-ether-modal',
  templateUrl: './get-ether-modal.component.html',
  styleUrls: ['./get-ether-modal.component.css']
})
export class GetEtherModalComponent {

  private projectModalRef: MatDialogRef<TransactionAwaitingModalComponent>;

  constructor(private balanceApiClient: BalanceApiClient,
              private authenticationService: AuthenticationService,
              private router: Router,
              private web3Service: Web3Service,
              private projectModal: MatDialog,
              private notificationsService: NotificationsService) {
  }

  public async receiveEth() {
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
      this.projectModal.closeAll();
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

}
