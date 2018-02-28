import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertsCountSelectionModalData} from './experts-count-selection-modal-data';
import {AreaType} from '../../../api/scoring/area-type.enum';

@Component({
  selector: 'app-experts-count-selection-modal',
  templateUrl: './experts-count-selection-modal.component.html',
  styleUrls: ['./experts-count-selection-modal.component.css']
})
export class ExpertsCountSelectionModalComponent {

  public ExpertiseAreaType: typeof AreaType = AreaType;

  constructor(@Inject(MAT_DIALOG_DATA) public data: ExpertsCountSelectionModalData,
              private dialogRef: MatDialogRef<ExpertsCountSelectionModalComponent>) {
  }

  submit() {
    this.dialogRef.close(this.data.settings.map(s => s.expertsCount));
  }
}
