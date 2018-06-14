import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';

@Component({
  selector: 'app-start-allotment-event-modal',
  templateUrl: './start-allotment-event-modal.component.html',
  styleUrls: ['./start-allotment-event-modal.component.scss']
})
export class StartAllotmentEventModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEventResponse,
              private dialogRef: MatDialogRef<AddAdminModalComponent>) {
  }

  ngOnInit() {
  }

  public submit(result) {
    this.dialogRef.close(result);
  }
}
