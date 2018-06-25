import {Component} from '@angular/core';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-delete-allotment-event-modal',
  templateUrl: './delete-allotment-event-modal.component.html',
  styleUrls: ['./delete-allotment-event-modal.component.scss']
})
export class DeleteAllotmentEventModalComponent {

  constructor(private dialog: MatDialogRef<DeleteAllotmentEventModalComponent>) {
  }

  public submit() {
    this.dialog.close(true);
  }
}
