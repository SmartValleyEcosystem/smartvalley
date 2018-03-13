import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {UserApiClient} from '../../../api/user/user-api-client';
import {AreaService} from '../../../services/expert/area.service';
import {FormGroup, Validators, FormBuilder} from '@angular/forms';
import {EditExpertRequest} from '../../../api/expert/edit-expert-request';
import {Area} from '../../../services/expert/area';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

  public selectedCategories = [];
  public saveBackendForm: FormGroup;
  public saveBlockchainForm: FormGroup;
  public editExpertRequest: EditExpertRequest;
  public areas: Area[] = this.areaService.areas;

  constructor(private expertApiClient: ExpertApiClient,
              private userApiClient: UserApiClient,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private areaService: AreaService,
              private formBuilder: FormBuilder,
              private dialogCreateExpert: MatDialogRef<EditExpertModalComponent>,
              private expertsRegistryContractClient: ExpertsRegistryContractClient) {
  }

  ngOnInit() {
    this.saveBackendForm = this.formBuilder.group({
      address: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', Validators.required],
      name: ['', Validators.required],
      about: ['']
    });
    let blockchainFormInputs = {
      available: [''],
    };
    const categoryBlockchainElements = this.areas.map((a, i, ar) => {
      const currentField: {} = {};
      currentField['category' + a.areaType] = [''];
      blockchainFormInputs = Object.assign(blockchainFormInputs, currentField);
    });
    this.saveBlockchainForm = this.formBuilder.group(blockchainFormInputs);
    const areasId = this.areaService.getAreasIdByNames(this.data.areas);

    for (let i = 0; areasId.length >= i; i++) {
      if (areasId.includes(i)) {
        this.selectedCategories[i] = true;
      }
    }
  }

  public SaveBlockchain() {
    this.dialogCreateExpert.close();
  }

  async submit(form, needToUpdateInfoInBlockchain = true) {
    const address = this.saveBackendForm.value.address;
    const email = this.saveBackendForm.value.email;
    const name = this.saveBackendForm.value.name;
    const about = this.saveBackendForm.value.about;
    const isAvailable = this.saveBlockchainForm.value.available || false;
    const categoriesToRequest: number[] = [];
    this.selectedCategories.map((value, index) => {
      if (value === true) {
        categoriesToRequest.push(index);
      }
    });

    this.editExpertRequest = {
      address: address,
      email: email,
      name: name,
      about: about,
      isAvailable: isAvailable,
      areas: categoriesToRequest
    };

    if (needToUpdateInfoInBlockchain) {
      let transactionHash: string;
      if (isAvailable) {
        transactionHash = (await this.expertsRegistryContractClient.enableAsync(address));
      } else {
        transactionHash = (await this.expertsRegistryContractClient.disableAsync(address));
      }
      this.editExpertRequest.transactionHash = transactionHash;
    }

    await this.expertApiClient.editExpertAsync(this.editExpertRequest);
    this.dialogCreateExpert.close();
  }
}
