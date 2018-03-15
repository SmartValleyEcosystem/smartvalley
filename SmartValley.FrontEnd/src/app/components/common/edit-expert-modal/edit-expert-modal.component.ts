import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {UserApiClient} from '../../../api/user/user-api-client';
import {AreaService} from '../../../services/expert/area.service';
import {FormGroup, Validators, FormBuilder} from '@angular/forms';
import {ExpertUpdateRequest} from '../../../api/expert/expert-update-request';
import {Area} from '../../../services/expert/area';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {ExpertResponse} from "../../../api/expert/expert-response";
import {EditExpertModalData} from "./edit-expert-modal-data";

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

  public selectedCategories = [];
  public backendForm: FormGroup;
  public areas: Area[] = this.areaService.areas;
  public expertDetails: ExpertResponse;
  public isAvailable: boolean;

  constructor(private expertApiClient: ExpertApiClient,
              @Inject(MAT_DIALOG_DATA) public data: EditExpertModalData,
              private areaService: AreaService,
              private formBuilder: FormBuilder,
              private dialogCreateExpert: MatDialogRef<EditExpertModalComponent>,
              private expertsRegistryContractClient: ExpertsRegistryContractClient) {
  }

  async ngOnInit() {
    this.backendForm = this.formBuilder.group({
      address: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', Validators.required],
      name: ['', Validators.required],
      about: ['']
    });

    this.expertDetails = await this.expertApiClient.getAsync(this.data.address);

    this.backendForm.setValue({
      address: this.expertDetails.address,
      name: this.expertDetails.name,
      about: this.expertDetails.about,
      email: this.expertDetails.email
    });

    this.isAvailable = this.expertDetails.isAvailable;
    const areasId = this.expertDetails.areas.map(a => a.id);

    for (const areaId of areasId) {
      this.selectedCategories[areaId] = true;
    }
  }

  async submitAsync(needToUpdateInBlockchain: boolean) {
    const address = this.backendForm.value.address;
    const email = this.backendForm.value.email;
    const name = this.backendForm.value.name;
    const about = this.backendForm.value.about;
    const isAvailable = this.isAvailable || false;

    const categoriesToRequest: number[] = [];
    this.selectedCategories.map((value, index) => {
      if (value === true) {
        categoriesToRequest.push(index);
      }
    });

    const editExpertRequest = <ExpertUpdateRequest> {
      address: address,
      email: email,
      name: name,
      about: about,
      isAvailable: isAvailable,
      areas: categoriesToRequest
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

    await this.expertApiClient.updateAsync(editExpertRequest);
    this.dialogCreateExpert.close();
  }
}
