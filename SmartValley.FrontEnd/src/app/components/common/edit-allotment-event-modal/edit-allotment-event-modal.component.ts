import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';
import {AllotmentEventResponse} from '../../../api/allotment-events/responses/allotment-event-response';
import {SVValidators} from '../../../utils/sv-validators';
import {EditAllotmentRequest} from '../../../api/allotment-events/request/edit-allotment-request';

@Component({
  selector: 'app-edit-allotment-event-modal',
  templateUrl: './edit-allotment-event-modal.component.html',
  styleUrls: ['./edit-allotment-event-modal.component.scss']
})
export class EditAllotmentEventModalComponent implements OnInit {
   public form: FormGroup;
   public allowedProjects: SelectItem[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: AllotmentEventResponse,
              private formBuilder: FormBuilder,
              private projectApiClient: ProjectApiClient,
              private erc223ContractClient: Erc223ContractClient,
              private subscribeModalComponent: MatDialogRef<EditAllotmentEventModalComponent>) {
  }

  async ngOnInit() {
      this.form = this.formBuilder.group({
          eventName: [this.data.name, [Validators.required]],
          tokenAddress: [this.data.tokenContractAddress, [Validators.minLength(42), Validators.maxLength(42)]],
          ticker: [this.data.tokenTicker, [Validators.minLength(6)]],
          tokenDecimals: [this.data.tokenDecimals, [Validators.required, Validators.min(0), Validators.max(18)]],
          finishDate: [new Date(this.data.finishDate), [SVValidators.checkFutureDate]],
      });

      const myProjectResponse = await this.projectApiClient.getMyProjectAsync();
      this.allowedProjects.push({
          label: myProjectResponse.name,
          value: myProjectResponse.id
      });
  }

  public submitForm() {
      if (this.form.valid) {
          this.subscribeModalComponent.close(
              <EditAllotmentRequest>this.form.value
          );
      }
  }

  public async checkToken(event) {
      if (event.target.value.length === 42) {
          this.form.controls['tokenDecimals'].setValue(await this.erc223ContractClient.getDecimalsAsync(event.target.value));
          this.form.controls['ticker'].setValue(await this.erc223ContractClient.getSymbolAsync(event.target.value));
      }
  }
}
