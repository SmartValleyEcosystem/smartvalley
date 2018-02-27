import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {SetExpertModalData} from './set-expert-modal-data';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';

@Component({
  selector: 'app-set-experts-modal',
  templateUrl: './set-experts-modal.component.html',
  styleUrls: ['./set-experts-modal.component.css']
})
export class SetExpertsModalComponent implements OnInit {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: SetExpertModalData,
              private dialogRef: MatDialogRef<SetExpertsModalComponent>) {
  }

  async ngOnInit() {
    const experts = [];
    for (const item of this.data.areas) {
      const group = this.formBuilder.group({
        areaType: item.id,
        title: item.name,
        address: ''
      });
      experts.push(group);
    }

    this.form = this.formBuilder.group({
      categories: this.formBuilder.array(experts)
    });
  }

  setExperts(form) {
    const areasExperts = form.value.categories;
    this.dialogRef.close(areasExperts);
  }
}
