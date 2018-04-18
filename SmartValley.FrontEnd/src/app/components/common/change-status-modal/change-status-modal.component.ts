import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {AlertModalData} from '../alert-modal/alert-modal-data';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';

@Component({
  selector: 'app-change-status-modal',
  templateUrl: './change-status-modal.component.html',
  styleUrls: ['./change-status-modal.component.css']
})
export class ChangeStatusModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data,
              private expertApiClient: ExpertApiClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient) { }

  ngOnInit() {
  }

  public async switchActivityAsync(activity, address) {
    let transactionHash = '';
    if (activity) {
      transactionHash = await this.expertsRegistryContractClient.enableAsync(address);
    } else {
      transactionHash = await this.expertsRegistryContractClient.disableAsync(address);
    }

    await this.expertApiClient.switchAvailabilityAsync(transactionHash, activity);
  }

}
