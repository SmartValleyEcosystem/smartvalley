import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {FormGroup, Validators, FormBuilder} from '@angular/forms';
import {ExpertResponse} from '../../../api/expert/expert-response';
import {EditExpertModalData} from './edit-expert-modal-data';
import {AdminApiClient} from '../../../api/admin/admin-api-client';
import {AdminExpertUpdateRequest} from '../../../api/admin/admin-expert-update-request';
import {Area} from '../../../services/expert/area';
import {AreaService} from '../../../services/expert/area.service';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {AdminSetAvailabilityRequest} from '../../../api/admin/admin-set-availability-request';
import {AdminExpertUpdateAreasRequest} from '../../../api/admin/admin-expert-update-areas-request';

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

  public selectedCategories = [];
  public startedCategories = [];
  public backendForm: FormGroup;
  public areas: Area[] = this.areaService.areas;
  public expertDetails: ExpertResponse = <ExpertResponse> {
    about: '',
    address: '',
    email: '',
    isAvailable: true,
    firstName: '',
    secondName: '',
    areas: []
  };
  public isAvailable: boolean;

  constructor(private adminApiClient: AdminApiClient,
              private expertApiClient: ExpertApiClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private areaService: AreaService,
              @Inject(MAT_DIALOG_DATA) public data: EditExpertModalData,
              private formBuilder: FormBuilder,
              private dialogCreateExpert: MatDialogRef<EditExpertModalComponent>) {
  }

  async ngOnInit() {
    this.backendForm = this.formBuilder.group({
      address: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', Validators.required],
      firstName: [''],
      secondName: [''],
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

    this.expertDetails.areas.map(a => {
      this.selectedCategories[a.id] = true;
      this.startedCategories[a.id] = true;
    });
  }

  public async submitExpertSettingsAsync(): Promise<void> {
    await Promise.all([
      this.submitPersonalSettingsAsync(),
      this.updateAreasAsync(),
      this.updateAvailabilityAsync()
    ]);
  }

  private async updateAreasAsync(): Promise<void> {

    const allCategories = this.selectedCategories.map((value, index) => index);
    const oldCategories = this.startedCategories.map((value, index) => index);
    const newCategories = allCategories.filter(i => oldCategories.indexOf(i) < 0);
    const categoriesToRequest = Object.keys(allCategories).map(i => +i);

    if (newCategories.length > 0) {

      const address = this.backendForm.value.address;

      const transactionHash = await this.expertsRegistryContractClient.addAsync(address, newCategories);

      const adminExpertUpdateAreasRequest = <AdminExpertUpdateAreasRequest>{
        address: address,
        transactionHash: transactionHash,
        areas: categoriesToRequest
      };

      await this.adminApiClient.updateExpertAreasAsync(adminExpertUpdateAreasRequest);
    }
  }

  private async updateAvailabilityAsync(): Promise<void> {

    const address = this.backendForm.value.address;
    const isAvailable = this.isAvailable || false;
    if (isAvailable !== this.expertDetails.isAvailable) {

      let transactionHash: string;

      if (isAvailable) {
        transactionHash = await this.expertsRegistryContractClient.enableAsync(address);
      } else {
        transactionHash = await this.expertsRegistryContractClient.disableAsync(address);
      }

      const adminSetAvailabilityRequest = <AdminSetAvailabilityRequest>{
        address: address,
        transactionHash: transactionHash,
        value: this.isAvailable
      };
      await this.adminApiClient.setExpertAvailabilityAsync(adminSetAvailabilityRequest);
    }
  }


  public async submitPersonalSettingsAsync() {
    const editExpertRequest = <AdminExpertUpdateRequest> {
      address: this.backendForm.value.address,
      email: this.backendForm.value.email,
      firstName: this.backendForm.value.firstName,
      secondName: this.backendForm.value.secondName,
      about: this.backendForm.value.about,
      isAvailable: this.isAvailable || false
    };

    await this.adminApiClient.updateExpertAsync(editExpertRequest);
    this.dialogCreateExpert.close();
  }
}
