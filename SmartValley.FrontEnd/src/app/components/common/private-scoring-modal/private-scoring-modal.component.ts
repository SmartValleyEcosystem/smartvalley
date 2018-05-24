import { Component, OnInit } from '@angular/core';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-private-scoring-modal',
  templateUrl: './private-scoring-modal.component.html',
  styleUrls: ['./private-scoring-modal.component.css']
})
export class PrivateScoringModalComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<AddAdminModalComponent>) { }

  ngOnInit() {
  }

  public submit(apply) {
      this.dialogRef.close(apply);
  }

}
