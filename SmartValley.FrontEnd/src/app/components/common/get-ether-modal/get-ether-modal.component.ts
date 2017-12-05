import {Component, EventEmitter, Input, Output} from '@angular/core';
import {BalanceApiClient} from '../../../api/balance/balance-api-client';
import {MatDialog} from '@angular/material';

@Component({
  selector: 'app-get-ether-modal',
  templateUrl: './get-ether-modal.component.html',
  styleUrls: ['./get-ether-modal.component.css']
})
export class GetEtherModalComponent {

  @Output() onClickReceive: EventEmitter<any> = new EventEmitter();
  constructor(private projectModal: MatDialog) {
  }

  public async receiveEth() {
    this.onClickReceive.emit();
    this.projectModal.closeAll();
  }
}
