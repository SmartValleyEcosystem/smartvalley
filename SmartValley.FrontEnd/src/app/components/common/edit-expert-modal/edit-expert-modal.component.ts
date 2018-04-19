import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {FormGroup, Validators, FormBuilder} from '@angular/forms';
import {ExpertResponse} from '../../../api/expert/expert-response';
import {EditExpertModalData} from './edit-expert-modal-data';
import {AdminApiClient} from '../../../api/admin/admin-api-client';
import {AdminExpertUpdateRequest} from '../../../api/admin/admin-expert-update-request';

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

  public backendForm: FormGroup;
  public expertDetails: ExpertResponse = <ExpertResponse> {
    about: '',
    address: '',
    email: '',
    isAvailable: true,
    firstName: '',
    secondName: ''
  };
  public isAvailable: boolean;

  constructor(private adminApiClient: AdminApiClient,
              private expertApiClient: ExpertApiClient,
              @Inject(MAT_DIALOG_DATA) public data: EditExpertModalData,
              private formBuilder: FormBuilder,
              private dialogCreateExpert: MatDialogRef<EditExpertModalComponent>) {
  }

  async ngOnInit() {
    this.backendForm = this.formBuilder.group({
      address: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', Validators.required],
      firstName: ['', Validators.required],
      secondName: ['', Validators.required],
      about: ['']
    });

    this.expertDetails = await this.expertApiClient.getAsync(this.data.address);

    this.backendForm.setValue({
      address: this.expertDetails.address,
      firstName: this.expertDetails.firstName,
      secondName: this.expertDetails.secondName,
      about: this.expertDetails.about,
      email: this.expertDetails.email
    });

    this.isAvailable = this.expertDetails.isAvailable;
  }

  async submitAsync(needToUpdateInBlockchain: boolean) {
    const address = this.backendForm.value.address;
    const email = this.backendForm.value.email;
    const firstName = this.backendForm.value.firstName;
    const secondName = this.backendForm.value.secondName;
    const about = this.backendForm.value.about;
    const isAvailable = this.isAvailable || false;

    const editExpertRequest = <AdminExpertUpdateRequest> {
      address: address,
      email: email,
      firstName: firstName,
      secondName: secondName,
      about: about,
      isAvailable: isAvailable
    };

    // https://rassvet-capital.atlassian.net/browse/ILT-730
    // if (needToUpdateInBlockchain) {
    //   let transactionHash: string;
    //   if (isAvailable) {
    //     transactionHash = (await this.expertsRegistryContractClient.enableAsync(address));
    //   } else {
    //     transactionHash = (await this.expertsRegistryContractClient.disableAsync(address));
    //   }
    //   editExpertRequest.transactionHash = transactionHash;
    // }

    await this.adminApiClient.updateExpertAsync(editExpertRequest);
    this.dialogCreateExpert.close();
  }
}
