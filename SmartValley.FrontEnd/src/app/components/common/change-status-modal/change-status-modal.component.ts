import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {AlertModalData} from '../alert-modal/alert-modal-data';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {DeleteProjectModalComponent} from '../delete-project-modal/delete-project-modal.component';

@Component({
  selector: 'app-change-status-modal',
  templateUrl: './change-status-modal.component.html',
  styleUrls: ['./change-status-modal.component.css']
})
export class ChangeStatusModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data,
              private expertApiClient: ExpertApiClient,
              private dialogChangeStatus: MatDialogRef<ChangeStatusModalComponent>) { }

  ngOnInit() {
  }

  public submit(changeStatus: boolean) {
    this.dialogChangeStatus.close(changeStatus);
  }

}
