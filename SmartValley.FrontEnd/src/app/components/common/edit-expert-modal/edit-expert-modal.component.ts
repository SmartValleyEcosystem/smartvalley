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
import {OffersApiClient} from '../../../api/scoring-offer/offers-api-client';
import {OffersQuery} from '../../../api/scoring-offer/offers-query';
import {ScoringOfferResponse} from '../../../api/scoring-offer/scoring-offer-response';

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

  public selectedCategories = [];
  public startedCategories = [];
  public disabledCategories = [];
  public backendForm: FormGroup;
  public areas: Area[] = this.areaService.areas;
  public offers: ScoringOfferResponse[] = [];
  public expertDetails: ExpertResponse = <ExpertResponse> {
    about: '',
    bitcointalk: '',
    address: '',
    email: '',
    isAvailable: true,
    isInHouse: false,
    firstName: '',
    secondName: '',
    areas: []
  };

  constructor(private adminApiClient: AdminApiClient,
              private expertApiClient: ExpertApiClient,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private areaService: AreaService,
              private offersApiClient: OffersApiClient,
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
      about: [''],
      bitcointalk: ['', Validators.pattern('https?://.+')],
      isInHouse: [false],
      isAvailable: [false]
    });

    this.expertDetails = await this.expertApiClient.getByAddressAsync(this.data.address);

    this.backendForm.setValue({
      address: this.expertDetails.address,
      firstName: this.expertDetails.firstName,
      secondName: this.expertDetails.secondName,
      about: this.expertDetails.about,
      email: this.expertDetails.email,
      isInHouse: this.expertDetails.isInHouse,
      bitcointalk: this.expertDetails.bitcointalk,
      isAvailable: this.expertDetails.isAvailable
    });

    this.expertDetails.areas.map(a => {
      this.selectedCategories[a.id] = true;
      this.startedCategories[a.id] = true;
    });

    const response = await this.offersApiClient.queryAsync(<OffersQuery>{
      offset: 0,
      count: 100,
      expertId: this.expertDetails.id
    });
    this.offers = response.items;
    this.offers.map(i => {
      this.disabledCategories[i.area] = true;
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
    const isAvailable = this.backendForm.value.isAvailable || false;
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
        value: isAvailable
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
      bitcointalk: this.backendForm.value.bitcointalk,
      isInHouse: this.backendForm.value.isInHouse,
      isAvailable: this.backendForm.value.isAvailable || false
    };

    await this.adminApiClient.updateExpertAsync(editExpertRequest);
    this.dialogCreateExpert.close();
  }
}
