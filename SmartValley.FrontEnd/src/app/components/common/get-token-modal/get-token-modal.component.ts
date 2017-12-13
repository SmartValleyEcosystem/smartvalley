import {Component, EventEmitter, Output} from '@angular/core';
import {MatDialog} from '@angular/material';

@Component({
  selector: 'app-get-token-modal',
  templateUrl: './get-token-modal.component.html',
  styleUrls: ['./get-token-modal.component.css']
})
export class GetSVTModalComponent {

  @Output() onClickReceive: EventEmitter<any> = new EventEmitter();
  constructor(private projectModal: MatDialog) {
  }

  public async receiveEth() {
    this.onClickReceive.emit();
    this.projectModal.closeAll();
  }
}
