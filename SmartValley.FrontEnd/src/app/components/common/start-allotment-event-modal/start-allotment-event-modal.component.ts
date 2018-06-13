import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AllotmentEvent} from '../../../api/allotment-events/allotment-event';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';

@Component({
  selector: 'app-start-allotment-event-modal',
  templateUrl: './start-allotment-event-modal.component.html',
  styleUrls: ['./start-allotment-event-modal.component.scss']
})
export class StartAllotmentEventModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEvent,
              private dialogRef: MatDialogRef<AddAdminModalComponent>) {
  }

  ngOnInit() {
  }

  public submit(result) {
    this.dialogRef.close(result);
  }
}
