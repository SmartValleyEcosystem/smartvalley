import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-delete-project-modal',
  templateUrl: './delete-project-modal.component.html',
  styleUrls: ['./delete-project-modal.component.css']
})
export class DeleteProjectModalComponent implements OnInit {

  constructor(private dialogCreateExpert: MatDialogRef<DeleteProjectModalComponent>) {
  }

  ngOnInit() {
  }

  public submit(toDelete: boolean) {
    this.dialogCreateExpert.close(toDelete);
  }

}
