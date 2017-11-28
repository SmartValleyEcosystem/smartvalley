import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-project-creating-modal',
  templateUrl: './project-creating-modal.component.html',
  styleUrls: ['./project-creating-modal.component.css']
})
export class ProjectCreatingModalComponent {

  constructor(public thisDialogRef: MatDialogRef<ProjectCreatingModalComponent>, @Inject(MAT_DIALOG_DATA) public data: string) {
  }
}
