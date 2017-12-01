import {Component} from '@angular/core';
import {DialogService} from '../../../services/dialog-service';

@Component({
  selector: 'app-get-ether-modal',
  templateUrl: './get-ether-modal.component.html',
  styleUrls: ['./get-ether-modal.component.css']
})
export class GetEtherModalComponent {

  constructor(private dialogService: DialogService) {
  }

  public async receiveEth() {
    await this.dialogService.showReceiveEthDialog();
  }
}
